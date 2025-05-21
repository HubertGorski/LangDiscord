using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using LangDiscord.Enums;
using LangDiscord.Extensions;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;

namespace LangDiscord.Services
{
    public class BotService : IBotService
    {
        private readonly DiscordSocketClient _client;
        private readonly DiscordData _discordData;
        private readonly ICommandExecutorService _commandExecutor;
        private readonly IUserTranslationCache _translationCache;
        private readonly IFavoriteService _favoriteService;
        private readonly IUsersTranslationCache _usersTranslationCache;
        private readonly InteractionService _commands;

        public BotService(
            DiscordSocketClient client,
            DiscordData discordData,
            ICommandExecutorService commandExecutor,
            IUserTranslationCache translationCache,
            IUsersTranslationCache usersTranslationCache,
            IFavoriteService favoriteService,
            InteractionService commands)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.Log += LogAsync;
            _client.MessageReceived += HandleMessageAsync;
            _client.ReactionAdded += OnReactionAdded;

            _discordData = discordData ?? throw new ArgumentNullException(nameof(discordData));
            _commandExecutor = commandExecutor ?? throw new ArgumentNullException(nameof(commandExecutor));
            _favoriteService = favoriteService ?? throw new ArgumentNullException(nameof(favoriteService));
            _translationCache = translationCache ?? throw new ArgumentNullException(nameof(translationCache));
            _usersTranslationCache = usersTranslationCache ?? throw new ArgumentNullException(nameof(usersTranslationCache));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public async Task RunBotAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _discordData.Token);
            await _client.StartAsync();

            _client.Ready += async () =>
            {
                await _commands.RegisterCommandsToGuildAsync(_discordData.ChannelId);
            };

            _client.InteractionCreated += async (SocketInteraction interaction) =>
            {
                if (interaction is SocketMessageComponent component)
                {
                    await HandleButtonInteraction(component);
                }
                else
                {
                    var context = new SocketInteractionContext(_client, interaction);
                    await _commands.ExecuteCommandAsync(context, null);
                }
            };

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task HandleMessageAsync(SocketMessage message)
        {
            if (!IsMessageFromAllowedSource(message)) return;

            if (!await HandlePendingTranslationResponseAsync(message)) return;

            UserRequest? userRequest = GetUserRequest(message);
            if (userRequest is null) return;

            await _commandExecutor.Execute(userRequest);
        }

        private bool IsMessageFromAllowedSource(SocketMessage message)
            => message is SocketUserMessage && !message.Author.IsBot && message.Channel.Id == _discordData.ChannelId;

        private bool IsReactionFromAllowedSource(SocketReaction reaction, string emoticon)
            => reaction.Emote.Name == emoticon && !reaction.User.Value.IsBot && reaction.Channel.Id == _discordData.ChannelId;

        private static UserRequest? GetUserRequest(SocketMessage message)
        {
            if (!message.Content.StartsWith("!")) return null;

            string content = message.Content[1..];
            string[] request = content.Split(' ');

            return new()
            {
                UserId = message.Author.Id,
                CommandText = request[0],
                Content = request.Length > 1 ? content.Substring(request[0].Length).TrimStart() : null,
                Message = message
            };
        }

        private async Task<bool> HandlePendingTranslationResponseAsync(SocketMessage message)
        {
            if (_translationCache.TryGetPendingTranslation(message.Author.Id, out var translation) && translation != null)
            {
                ulong messageId = await MessageHelper.SendMessageToUser(message, translation.Answer);

                _translationCache.TryRemovePendingTranslation(message.Author.Id, out _);
                TranslationResultForUser translationCopy = DeepClone(translation);
                translationCopy.IsAnswerCard = true;
                _usersTranslationCache.AddTranslation(messageId, translationCopy);
                return false;
            }
            return true;
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message == null) return;

            if (IsReactionFromAllowedSource(reaction, "❤️"))
            {
                _favoriteService.AddFavorite(reaction.UserId, message);
                return;
            }
        }

        //TODO: https://dev.azure.com/hubertgorski181/HubProjects/_workitems/edit/1
        public static T DeepClone<T>(T obj)
        {
            string json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }

        private async Task HandleButtonInteraction(SocketMessageComponent component)
        {
            var customIdParts = component.Data.CustomId.Split(":");
            if (customIdParts.Length < 2)
            {
                await component.RespondAsync("❌ Button data error!", ephemeral: true);
                return;
            }

            var action = customIdParts[0];
            if (action == "remove_fav")
            {
                var userId = Convert.ToUInt64(customIdParts[1]);
                var messageId = component.Message.Id;

                if (component.User.Id != userId)
                {
                    await component.RespondAsync("❌ You can't do this!", ephemeral: true);
                    return;
                }

                if (_favoriteService.RemoveFavorite(userId, messageId))
                {

                    await component.UpdateAsync(x =>
                    {
                        x.Content = "✅ Removed from favorites!";
                        x.Components = new ComponentBuilder().Build();
                    });
                }
            }
            else
            {
                await component.RespondAsync("❌ Unknown action!", ephemeral: true);
            }
        }
    }
}
