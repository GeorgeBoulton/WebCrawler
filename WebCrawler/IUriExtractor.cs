using System.Collections.Generic;

namespace WebCrawler
{
    public interface IUriExtractor
    {
        IEnumerable<string> GetUris(string content);
    }
}