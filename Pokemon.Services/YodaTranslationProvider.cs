using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pokemon.Services.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public class YodaTranslationProvider : IYodaTranslationProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<YodaTranslationProvider> _logger;

        public YodaTranslationProvider(HttpClient httpClient, ILogger<YodaTranslationProvider> logger)
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
