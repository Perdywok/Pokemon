using System.Linq;

namespace Pokemon.Services.Models
{
    public record PokemonResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        public static explicit operator PokemonResponse(RawPokemonResponse responseFromApi)
        {
            var pokemon = new PokemonResponse
            {
                Name = responseFromApi.name,
                Description = responseFromApi.flavor_text_entries
                .FirstOrDefault(f => f.language?.name == "en")?.flavor_text,
                Habitat = responseFromApi.habitat?.name,
                IsLegendary = responseFromApi.is_legendary
            };

            // it breaks requests to translation APIs
            // coz we cannot include newline symbol in GET request.
            // Buying of subscription and using POST with apikey instead may resolve the problem.
            // TODO: find how to fix it in more elegant way if possible.
            pokemon.Description = pokemon.Description.Replace("\n", " ");

            return pokemon;
        }
    }
}
