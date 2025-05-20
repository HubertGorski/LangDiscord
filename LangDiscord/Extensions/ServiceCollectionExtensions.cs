using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Handlers;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Services;
using LangDiscord.Services.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LangDiscord.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomCaches(this IServiceCollection services, IConfiguration config)
        {

            services.AddSingleton<IUserFavoriteTranslationCache>(_ =>
            new UserFavoriteTranslationCache(
                ConfigHelper.GetCacheDuration(config, "CacheSettings:FavoriteTranslationCacheDuration"),
                ConfigHelper.GetCacheDuration(config, "CacheSettings:FavoriteTranslationCacheCleanupInterval")
            ));

            services.AddSingleton<IUsersTranslationCache>(_ =>
                new UsersTranslationCache(
                    ConfigHelper.GetCacheDuration(config, "CacheSettings:AllTranslationsCacheDuration"),
                    ConfigHelper.GetCacheDuration(config, "CacheSettings:AllTranslationsCacheCleanupInterval")
                ));

            services.AddSingleton<IUserTranslationCache>(_ =>
                new UserTranslationCache(
                    ConfigHelper.GetCacheDuration(config, "CacheSettings:TranslationCacheDuration"),
                    ConfigHelper.GetCacheDuration(config, "CacheSettings:TranslationCacheCleanupInterval")
                ));

            services.AddSingleton<IUserSettingsCache>(_ =>
                new UserSettingsCache(
                    ConfigHelper.GetCacheDuration(config, "CacheSettings:SettingsCacheDuration"),
                    ConfigHelper.GetCacheDuration(config, "CacheSettings:SettingsCacheCleanupInterval")
                ));

            return services;
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services.AddTransient<ICommandHandler, HelpHandler>();
            Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass &&
                          !t.IsAbstract &&
                          t.Namespace == "LangDiscord.Handlers" &&
                          typeof(ICommandHandler).IsAssignableFrom(t) &&
                          t != typeof(HelpHandler))
                .ToList()
                .ForEach(type => services.AddTransient(typeof(ICommandHandler), type));

            return services;
        }
    }
}
