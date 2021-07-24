using FluentAssertions;
using Moq;
using Pokemon.Services;
using Pokemon.Services.Enums;
using Pokemon.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Pokemon.Tests
{
    public class TranslationPokemonServiceTests
    {
        [Theory]
        [InlineData("cave", false, TranslationType.Yoda)]
        [InlineData("false", true, TranslationType.Yoda)]
        [InlineData("randomHabitat", false, TranslationType.Shakespeare)]
        public async Task GetTranslatedPokemon_PokemonExists_CorrectTranslationTypeIsChosen(string habitat, bool isLegendary, TranslationType expectedTranslationType)
        {
            // arrange
            var pokemonProvider = new Mock<IPokemonProvider>();
            pokemonProvider.Setup(p => p.GetPokemonAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonResponse { Habitat = habitat, IsLegendary = isLegendary });
            var phraseTranslator = new Mock<IPhraseTranslator>();
            phraseTranslator.Setup(p => p.Translate(expectedTranslationType, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TranslationResponse { IsSuccessful = true });
            var service = new TranslationPokemonService(pokemonProvider.Object, phraseTranslator.Object);
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            var result = await service.GetTranslatedPokemon("pokemonName", token);

            // assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetTranslatedPokemon_PokemonExists_CorrectDescriptionIsChosen(bool isSuccessful)
        {
            // arrange
            var pokemonProvider = new Mock<IPokemonProvider>();
            var pokemonResponse = new PokemonResponse { Description = "defaultDescription"};
            pokemonProvider.Setup(p => p.GetPokemonAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pokemonResponse);
            var phraseTranslator = new Mock<IPhraseTranslator>();
            var translationResponse = new TranslationResponse { IsSuccessful = isSuccessful, Text = "transatedDescription" };
            phraseTranslator.Setup(p => p.Translate(It.IsAny<TranslationType>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(translationResponse);
            var service = new TranslationPokemonService(pokemonProvider.Object, phraseTranslator.Object);
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            // act
            var result = await service.GetTranslatedPokemon("pokemonName", token);

            // assert
            var expectedDescription = isSuccessful ? translationResponse.Text : pokemonResponse.Description;
            result.Should().NotBeNull();
            result.Description.Should().Be(expectedDescription);
        }
    }
}
