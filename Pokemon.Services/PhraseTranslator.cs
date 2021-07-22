using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pokemon.Services.Enums;
using Pokemon.Services.Exceptions;
using Pokemon.Services.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public class PhraseTranslator : IPhraseTranslator
    {
        private readonly TranslationOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PhraseTranslator> _logger;

        public PhraseTranslator(IOptions<TranslationOptions> options, HttpClient httpClient, ILogger<PhraseTranslator> logger)
        {
            _options = options.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TranslationResponse> Translate(TranslationType type, string strToTranslate, CancellationToken token)
        {
            _options.TranslationUrls.TryGetValue(type.ToString(), out string url);
            if (string.IsNullOrEmpty(url))
            {
                _logger.LogError($"Unknown type of TranslationType enum. Value is {type}");
                throw new PokemonException("Translation error.");
            }

            var translationResponse = await _httpClient.GetAsync($"{url}?text={strToTranslate}", token);
            if (!translationResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"GET Shakespeare translation is failed for string {strToTranslate}. Status code is {translationResponse.StatusCode}");
                return new TranslationResponse { IsSuccessful = false };
            }

            var json = await translationResponse.Content.ReadAsStringAsync(token);
            var deserializedResponse = JsonConvert.DeserializeObject<RawTranslationResponse>(json);

            return (TranslationResponse)deserializedResponse;
        }
    }
}
