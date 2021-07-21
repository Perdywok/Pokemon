using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public interface IYodaTranslationProvider
    {
        Task<TranslationResponse> Translate(string strToTranslate, CancellationToken token);
    }
}
