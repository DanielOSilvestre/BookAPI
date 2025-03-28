using System.Text.Json.Serialization;

namespace BookInformationAggregatorAPI.ExternalModels
{
    public class OpenLibraryBook
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string>? AuthorName { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublicYear { get; set; }
    }
}
