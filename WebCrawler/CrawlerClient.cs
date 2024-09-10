using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawler
{
    public class CrawlerClient : ICrawlerClient
    {
        protected readonly HttpClient _httpClient;

        public CrawlerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetPageContent(Uri uri)
        {
            return await _httpClient.GetAsync(uri);
        }
    }
}


