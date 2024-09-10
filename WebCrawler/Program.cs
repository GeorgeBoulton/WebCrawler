using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

namespace WebCrawler
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices().BuildServiceProvider();
            var crawler = serviceProvider.GetService<ICrawler>();

            crawler.Crawl("https://monzo.com/").Wait();
        }

        private static ServiceCollection ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Configure logging
            serviceCollection.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddDebug();
                configure.AddFilter<ConsoleLoggerProvider>("System.Net.Http.HttpClient", LogLevel.Warning); // Exclude HttpClient logs from Console logger
                configure.AddFilter<DebugLoggerProvider>("System.Net.Http.HttpClient", LogLevel.Warning); // Exclude HttpClient logs from Debug logger
            });

            // Register services
            serviceCollection.AddSingleton<ICrawlerClient, CrawlerClient>();
            serviceCollection.AddSingleton<ICrawler, Crawler>();
            serviceCollection.AddSingleton<IUriExtractor, UriExtractor>();
            
            serviceCollection.AddHttpClient<CrawlerClient>();

            return serviceCollection;
        }
    }
}
