using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.Models
{
    public class Book
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("author")]
        public required string Author { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("published_year")]
        public int? PublishedYear { get; set; }
    }
}
