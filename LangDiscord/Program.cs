using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LangDiscord.Services;
using LangDiscord.Interfaces;
using LangDiscord.Handlers;
using LangDiscord.Enums;
using LangDiscord.Services.Cache;
using LangDiscord.Helpers;
using Microsoft.Extensions.Hosting;
using Discord.WebSocket;
using Discord;
using System.Net.Http;
using DetectLanguage;
using LangDiscord.Extensions;
using LangDiscord.Facades;
using Discord.Interactions;

namespace LangDiscord
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile($"appsettings.local.json", optional: true)
                        .AddJsonFile($"appsettings.secret.local.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    ConfigureServices(services, config);
                })
                .Build();

            var bot = host.Services.GetRequiredService<IBotService>();
            await bot.RunBotAsync();
        }


        private static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton(ConfigHelper.GetDiscordData(config));

            services.AddCustomHttpClients();

            services.AddCustomCaches(config);

            services.AddSingleton(new DiscordSocketClient(ConfigHelper.GetDiscordSocketConfig()));
            services.AddSingleton<ILangCodeConverter, LangCodeConverter>();
            services.AddSingleton<ICommandExtensions, CommandExtensions>();
            services.AddSingleton<IFavoriteService, FavoriteService>();
            services.AddSingleton<ICommandExecutorService, CommandExecutorService>();
            services.AddSingleton<ITranslationFacade, TranslationFacade>();
            services.AddSingleton<IUserLangSettingsFacade, UserLangSettingsFacade>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<InteractionService>(sp =>
            {
                var client = sp.GetRequiredService<DiscordSocketClient>();
                return new InteractionService(client.Rest);
            });

            services.AddSingleton<IBotService, BotService>();

            services.AddScoped<ITatoebaService, TatoebaService>();
            services.AddScoped<IMyMemoryService, MyMemoryService>();
            services.AddScoped<ILanguageDetectionService, LanguageDetectionService>();

            services.AddCommandHandlers();
        }
    }
}