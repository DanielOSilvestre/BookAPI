using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.ExternalModels
{
    public class OpenLibraryResponse
    {
        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("num_found")]
        public int? NumFound { get; set; }

        [JsonPropertyName("docs")]
        public List<OpenLibraryBook>? Docs { get; set; }
    }
}
