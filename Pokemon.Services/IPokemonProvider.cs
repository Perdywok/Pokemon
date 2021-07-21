using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public interface IPokemonProvider
    {
        /// <summary>
        /// Returns pokemon full description;
        /// </summary>
        /// <param name="name"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PokemonResponse> GetPokemonAsync(string name, CancellationToken token);
    }
}
