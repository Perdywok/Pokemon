using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Pokemon.Tests
{
    public class PokemonControllerTests
    : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public PokemonControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PokemonController_Get_ExistingPokemon_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "api/pokemon/ditto";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType.ToString()
                .Should().Be("application/json; charset=utf-8");
        }

        [Fact]
        public async Task PokemonController_Get_NonExistentPokemon_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "api/pokemon/someFunnyName";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
