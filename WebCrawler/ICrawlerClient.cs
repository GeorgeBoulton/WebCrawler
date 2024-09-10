using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebCrawler
{
    public interface ICrawlerClient
    {
        Task<HttpResponseMessage> GetPageContent(Uri uri);   
    }
}


