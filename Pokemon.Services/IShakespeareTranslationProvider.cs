using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    // reference https://funtranslations.com/api/#shakespeare
    public interface IShakespeareTranslationProvider
    {
        Task<TranslationResponse> Translate(string strToTranslate, CancellationToken token);
    }
}
