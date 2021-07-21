using System;
using System.Net;

namespace Pokemon.Services.Exceptions
{
    [Serializable]
    public class PokemonException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public PokemonException(string message)
            : base(message)
        {
        }
        public PokemonException(HttpStatusCode code, string message)
            : base(message)
        {
            StatusCode = code;
        }
    }
}
