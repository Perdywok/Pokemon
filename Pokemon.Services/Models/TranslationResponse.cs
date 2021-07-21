namespace Pokemon.Services.Models
{
    public class TranslationResponse
    {
        public bool IsSuccessful { get; set; }
        public string Text { get; set; }

        public static explicit operator TranslationResponse(RawTranslationResponse responseFromApi)
        {
            return new TranslationResponse
            {
                IsSuccessful = responseFromApi.success.total > 0,
                Text = responseFromApi.contents.translated
            };
        }
    }
}
