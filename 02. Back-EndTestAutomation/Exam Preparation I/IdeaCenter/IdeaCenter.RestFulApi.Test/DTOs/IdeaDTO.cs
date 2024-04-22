using System.Text.Json.Serialization;

namespace IdeaCenter.RestFulApi.Test.DTOs
{
    public class IdeaDTO
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
