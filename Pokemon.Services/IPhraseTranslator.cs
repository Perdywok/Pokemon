using Pokemon.Services.Enums;
using Pokemon.Services.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Services
{
    public interface IPhraseTranslator
    {
        Task<TranslationResponse> Translate(TranslationType type, string strToTranslate, CancellationToken token);
    }
}
