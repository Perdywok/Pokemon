using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public interface ITranslationPokemonService
    {
        Task<PokemonResponse> GetTranslatedPokemon(string name, CancellationToken token);
    }
}
