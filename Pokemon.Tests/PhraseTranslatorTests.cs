using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pokemon.Services;
using Pokemon.Services.Enums;
using Pokemon.Services.Exceptions;
using Pokemon.Services.Models;
using Pokemon.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;


namespace Pokemon.Tests
{
    public class PhraseTranslatorTests
    {
        [Fact]
        public async void PhraseTranslator_SuccessfulHttpResponse_ReturnsTranslation()
        {
            // arrange
            var urls = new Dictionary<string, string>
            {
                { "Shakespeare", "https://api.funtranslations.com/translate/shakespeare.json"}
            };
            var options = Options.Create(new TranslationOptions() { TranslationUrls = urls });

            var logger = Mock.Of<ILogger<PhraseTranslator>>();
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            var responseModel = new RawTranslationResponse
            {
                success = new Success { total = 1},
                contents = new Contents { translated = "some translated text"}
            };
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(responseModel))
            };

            var httpClient = HttpClientMocker.MockHttpClient(httpResponse);
            var translator = new PhraseTranslator(options, httpClient, logger);

            // act
            var translation = await translator.Translate(TranslationType.Shakespeare, "ditto", token);

            // assert
            translation.Should().NotBeNull();
            translation.IsSuccessful.Should().BeTrue();
            translation.Text.Should().Be(responseModel.contents.translated);
        }

        [Fact]
        public async void PhraseTranslator_IncorrectTranslationType_ThrowsInternalError()
        {
            // arrange
            var options = Options.Create(new TranslationOptions() {TranslationUrls = new() });
            var logger = Mock.Of<ILogger<PhraseTranslator>>();
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;
            var translator = new PhraseTranslator(options, null, logger);

            // act
            translator.Invoking(y => y.Translate(TranslationType.Shakespeare, "ditto", token))

            // assert
            .Should().Throw<PokemonException>()
            .Where(e => e.StatusCode == HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PhraseTranslator_UnsuccesssfulStatusCode_IsSuccessfulFalse()
        {
            // arrange
            var urls = new Dictionary<string, string>
            {
                { "Shakespeare", "https://api.funtranslations.com/translate/shakespeare.json"}
            };
            var options = Options.Create(new TranslationOptions() { TranslationUrls = urls });

            var logger = Mock.Of<ILogger<PhraseTranslator>>();
            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;
        
            var httpMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
            };

            var httpClient = HttpClientMocker.MockHttpClient(httpMessage);
            var translator = new PhraseTranslator(options, httpClient, logger);

            // act
            var translation = await translator.Translate(TranslationType.Shakespeare, "ditto", token);

            // assert
            translation.Should().NotBeNull();
            translation.IsSuccessful.Should().BeFalse();
        }
    }
}
