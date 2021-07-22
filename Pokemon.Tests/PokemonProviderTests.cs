using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Pokemon.Services;
using Pokemon.Services.Exceptions;
using Pokemon.Services.Models;
using Pokemon.Tests.Helpers;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pokemon.Tests
{
    public class PokemonProviderTests
    {
        [Fact]
        public async Task PokemonProvider_GetPokemonAsync_ReturnsDeserializedPokemon()
        {
            // arrange
            var responseModel = new RawPokemonResponse
            {
                name = "asd",
                flavor_text_entries = new List<FlavorTextEntry> { new FlavorTextEntry { language = new Language { name = "en" }, flavor_text = "sometext" } },
                habitat = new Habitat { name = "somename" },
                is_legendary = false
            };
            var message = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(responseModel))
            };

            var httpclient = HttpClientMocker.MockHttpClient(message);
            var logger = Mock.Of<ILogger<PokemonProvider>>();
            var provider = new PokemonProvider(httpclient, logger);

            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            var result = await provider.GetPokemonAsync("ditto", token);

            // assert
            result.Should().NotBeNull();
            result.Name.Should().Be(responseModel.name);
            result.Description.Should().Be(responseModel.flavor_text_entries[0].flavor_text);
            result.Habitat.Should().Be(responseModel.habitat.name);
            result.IsLegendary.Should().Be(responseModel.is_legendary);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.TooManyRequests, HttpStatusCode.ServiceUnavailable)]
        public async void PokemonProvider_GetPokemonAsync_ThrowsException(HttpStatusCode requestCode, HttpStatusCode exceptionCode)
        {
            // arrange
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = requestCode,
            };

            var httpClient = HttpClientMocker.MockHttpClient(httpResponse);
            var logger = Mock.Of<ILogger<PokemonProvider>>();
            var provider = new PokemonProvider(httpClient, logger);

            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            provider.Invoking(y => y.GetPokemonAsync("ditto", token))
            // assert
            .Should().Throw<PokemonException>()
            .Where(e => e.StatusCode == exceptionCode);
        }
    }
}
