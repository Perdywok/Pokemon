using Pokemon.Services.Enums;
using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    // represents facade pattern to remove logic from controller action method
    public class TranslationPokemonService : ITranslationPokemonService
    {
        private readonly IPokemonProvider _pokemonProvider;
        private readonly IPhraseTranslator _translator;

        public TranslationPokemonService(IPokemonProvider pokemonProvider, IPhraseTranslator translator)
        {
            _pokemonProvider = pokemonProvider;
            _translator = translator;
        }

        public async Task<PokemonResponse> GetTranslatedPokemon(string name, CancellationToken token)
        {
            PokemonResponse pokemon = await _pokemonProvider.GetPokemonAsync(name, token);
            // we should place TranslationType to database and get it from there instead of the enum.
            // but a database is not mentioned in test task so...
            TranslationType translationType;
            if (pokemon.Habitat == "cave" || pokemon.IsLegendary)
            {
                translationType = TranslationType.Yoda;
            }
            else
            {
                translationType = TranslationType.Shakespeare;
            }

            var translatedDescriptionResponse = await _translator.Translate(translationType, pokemon.Description, token);
            if (translatedDescriptionResponse.IsSuccessful)
            {
                pokemon.Description = translatedDescriptionResponse.Text;
            }

            return pokemon;
        }
    }
}
