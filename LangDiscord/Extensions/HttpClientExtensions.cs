using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LangDiscord.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddCustomHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient("MyMemory", ConfigureMyMemory);
            services.AddHttpClient("Tatoeba", ConfigureTatoeba);
            return services;
        }

        private static void ConfigureMyMemory(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.mymemory.translated.net");
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private static void ConfigureTatoeba(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.tatoeba.org/");
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
    }
}
