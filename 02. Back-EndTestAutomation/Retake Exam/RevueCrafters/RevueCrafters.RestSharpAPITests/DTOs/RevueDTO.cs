using System.Text.Json.Serialization;

namespace RevueCrafters.RestSharpAPI.Test.DTOs
{
    public class RevueDTO
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
