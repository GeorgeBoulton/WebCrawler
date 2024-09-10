using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class Crawler : ICrawler
    {
        private readonly ICrawlerClient _crawlerClient;
        private readonly IUriExtractor _uriExtractor;
        private readonly ILogger<Crawler> _logger;
        
        private string baseUri;
        public ConcurrentDictionary<string, byte> CrawledUris { get; } = new();

        public Crawler(ICrawlerClient crawlerClient, IUriExtractor uriExtractor, ILogger<Crawler> logger)
        {
            _crawlerClient = crawlerClient;
            _uriExtractor = uriExtractor;
            _logger = logger;
        }

        public async Task Crawl(string uri)
        {
            baseUri = uri;

            await CrawlPage(uri);
        }

        private async Task CrawlPage(string uri)
        {
            await Task.Delay(1000); // Rate limiter

            if (!CrawledUris.TryAdd(uri, 0)) return;
            
            var content = await GetPageContent(uri);

            if (string.IsNullOrEmpty(content)) return;

            var uris = _uriExtractor.GetUris(content).ToArray();

            if (uris?.Length == 0) return;

            _logger.LogInformation($"{uri} ---> {string.Join($"; ", uris)}");

            var crawlTasks = uris
                .Where(pageUri => pageUri.StartsWith(baseUri) && !CrawledUris.ContainsKey(pageUri))
                .Select(pageUri => CrawlPage(pageUri));

            await Task.WhenAll(crawlTasks); 
        }

        private async Task<string> GetPageContent(string uri)
        {
            try
            {
                var page = await _crawlerClient.GetPageContent(new Uri(uri));

                return await page.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get content for {uri}");

                return null;
            }
        }
    }
}
