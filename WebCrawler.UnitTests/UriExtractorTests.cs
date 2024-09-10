using Moq;
using System.Linq;
using Xunit;

namespace WebCrawler.UnitTests
{
    public class UriExtractorTests
    {
        private readonly UriExtractor _sut = new UriExtractor();

        [Fact]
        public void GetUris_WhenGivenHtml_ShouldReturnListOfUris()
        {
            // Arrange
            string content = "<link rel=\"alternate\" media=\"only screen and(max - width: 640px)\" href=\"https://m.crawler-test.com\"/>< link rel = \"alternate\" media = \"handheld\" href = \"https://crawler-test.com/links/alternate_media_handheld\" /> ";

            // Act
            var result = _sut.GetUris(content).ToArray();

            // Assert
            var expected = new [] { "https://m.crawler-test.com", "https://crawler-test.com/links/alternate_media_handheld" };

            Assert.Equal(2, result.Length);
            Assert.Equal(result, expected);
        }
    }
}
