using System.Threading.Tasks;

namespace WebCrawler
{
    public interface ICrawler
    {
        Task Crawl(string uri);
    }
}
