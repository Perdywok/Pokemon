using Moq;
using Moq.Protected;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Tests.Helpers
{
    public class HttpClientMocker
    {
        public static HttpClient MockHttpClient(HttpResponseMessage message)
        {
            //var mockClient = new Mock<HttpClient>();
            //mockClient.Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(message);

            //var client = mockClient.Object;
            //client.BaseAddress = new Uri("https://test/");
            //return client;
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => message);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test/")
            };

            return httpClient;
        }
    }
}
