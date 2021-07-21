using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pokemon.Services.Exceptions;
using Pokemon.Services.Models;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public class ShakespeareTranslationProvider : IShakespeareTranslationProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShakespeareTranslationProvider> _logger;

        public ShakespeareTranslationProvider(HttpClient httpClient, ILogger<ShakespeareTranslationProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TranslationResponse> Translate(string strToTranslate, CancellationToken token)
        {
            var translationResponse = await _httpClient.GetAsync($"?text={strToTranslate}", token);

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
