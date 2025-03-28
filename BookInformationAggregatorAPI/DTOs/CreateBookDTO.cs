using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.DTOs
{
    public class CreateBookDTO
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("published_year")]
        public int? PublishedYear { get; set; }
    }
}
