using Newtonsoft.Json;
using Pokemon.Services.Exceptions;
using Pokemon.Services.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pokemon.Services
{
    public class PokemonProvider : IPokemonProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PokemonProvider> _logger;

        public PokemonProvider(HttpClient httpClient, ILogger<PokemonProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PokemonResponse> GetPokemonAsync(string name, CancellationToken token)
        {
            // TODO: Implement caching via distributed cache storage like Redis.
            HttpResponseMessage response = await _httpClient.GetAsync($"pokemon-species/{name}", token);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new PokemonException(HttpStatusCode.NotFound, $"There is no pokemon with the name {name}.");
                }

                _logger.LogWarning($"GET pokemon-species is failed for name {name}");
                throw new PokemonException(HttpStatusCode.ServiceUnavailable, "Service is temporarily unavailable");
            }

            var json = await response.Content.ReadAsStringAsync(token);
            RawPokemonResponse deserializedResponse = JsonConvert.DeserializeObject<RawPokemonResponse>(json);

            return (PokemonResponse)deserializedResponse;
        }
    }
}
