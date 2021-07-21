using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    // represents facade pattern to remove logic from controller action method
    public class TranslationPokemonService : ITranslationPokemonService
    {
        private readonly IPokemonProvider _pokemonProvider;
        private readonly IYodaTranslationProvider _yodaTranslationProvider;
        private readonly IShakespeareTranslationProvider _shakespeareTranslationProvider;

        public TranslationPokemonService(IPokemonProvider pokemonProvider, IYodaTranslationProvider yodaTranslationProvider, IShakespeareTranslationProvider shakespeareTranslationProvider)
        {
            _shakespeareTranslationProvider = shakespeareTranslationProvider;
            _yodaTranslationProvider = yodaTranslationProvider;
            _pokemonProvider = pokemonProvider;
        }

        public async Task<PokemonResponse> GetTranslatedPokemon(string name, CancellationToken token)
        {
            var pokemon = await _pokemonProvider.GetPokemonAsync(name, token);
            if (pokemon.Habitat == "cave" || pokemon.IsLegendary)
            {
                var translatedDescriptionResponse = await _yodaTranslationProvider.Translate(pokemon.Description, token);
                if (translatedDescriptionResponse.IsSuccessful)
                {
                    pokemon.Description = translatedDescriptionResponse.Text;
                }
            }
            else
            {
                var translatedDescriptionResponse = await _shakespeareTranslationProvider.Translate(pokemon.Description, token);
                if (translatedDescriptionResponse.IsSuccessful)
                {
                    pokemon.Description = translatedDescriptionResponse.Text;
                }
            }

            return pokemon;
        }
    }
}
