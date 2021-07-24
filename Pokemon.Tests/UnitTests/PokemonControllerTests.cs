using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pokemon.Controllers;
using Pokemon.Services;
using Pokemon.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pokemon.Tests.UnitTests
{
    public class PokemonControllerTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Get_IncorrectName_ReturnsBadRequest(string name)
        {
            // arrange
            var controller = new PokemonController(null, null);
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            var result = await controller.Get(name, token);

            // assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Get_CorrectName_ReturnsOk()
        {
            // arrange
            var pokemonProvider = new Mock<IPokemonProvider>();
            pokemonProvider.Setup(p => p.GetPokemonAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonResponse());
            var controller = new PokemonController(pokemonProvider.Object, null);
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            var result = await controller.Get("someName", token);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetTranslated_IncorrectName_ReturnsBadRequest(string name)
        {
            // arrange
            var controller = new PokemonController(null, null);
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            var result = await controller.GetTranslated(name, token);

            // assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetTranslated_CorrectName_ReturnsOk()
        {
            // arrange
            var translationPokemonService = new Mock<ITranslationPokemonService>();
            translationPokemonService.Setup(p => p.GetTranslatedPokemon(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonResponse());
            var controller = new PokemonController(null, translationPokemonService.Object);
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            var result = await controller.GetTranslated("someName", token);

            // assert
            result.Should().BeAssignableTo<OkObjectResult>();
        }
    }
}
