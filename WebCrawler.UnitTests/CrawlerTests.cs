using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace WebCrawler.UnitTests
{
    public class CrawlerTests
    {
        private readonly Crawler _sut;
        private readonly Fixture _fixture = new Fixture();

        private readonly Mock<ICrawlerClient> _crawlerClientMock = new Mock<ICrawlerClient>();
        private readonly Mock<IUriExtractor> _uriExtractorMock = new Mock<IUriExtractor>();
        private readonly Mock<ILogger<Crawler>> _loggerMock = new Mock<ILogger<Crawler>>();


        public CrawlerTests()
        {
            _sut = new Crawler(_crawlerClientMock.Object, _uriExtractorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Crawl_ShouldCrawlPages_GivenValidUrisAndContent()
        {
            // Arrange
            var baseUri = "https://example.com";
            var uris = new [] { "https://example.com/page1", "https://example.com/page2" };
            var pageResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_fixture.Create<string>())
            };

            _uriExtractorMock.Setup(x => x.GetUris(It.IsAny<string>())).Returns(uris);
            _crawlerClientMock.Setup(x => x.GetPageContent(It.IsAny<Uri>()))
                .ReturnsAsync(pageResponse);

            // Act
            await _sut.Crawl(baseUri);

            // Assert
            _crawlerClientMock.Verify(x => x.GetPageContent(It.IsAny<Uri>()), Times.Exactly(3));
            _uriExtractorMock.Verify(x => x.GetUris(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
