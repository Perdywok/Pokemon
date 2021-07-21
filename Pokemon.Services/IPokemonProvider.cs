using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public interface IPokemonProvider
    {
        /// <summary>
        /// Returns pokemon description or null if it's not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PokemonResponse> GetPokemonAsync(string name, CancellationToken token);
    }
}
