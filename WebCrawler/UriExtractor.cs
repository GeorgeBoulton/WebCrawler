using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    public class UriExtractor : IUriExtractor
    {
        private readonly Regex linkParser = new(@"\bhttps?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IEnumerable<string> GetUris(string content)
        {
            return linkParser.Matches(content)
                .Select(m => m.Value)
                .ToArray();
        }
    }
}