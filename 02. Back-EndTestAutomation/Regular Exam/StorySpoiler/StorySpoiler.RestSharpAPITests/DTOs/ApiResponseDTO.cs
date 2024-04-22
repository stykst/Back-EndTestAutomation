using System.Net;
using System.Text.Json.Serialization;

namespace StorySpoiler.RestSharpAPITests.DTOs
{
    public class ApiResponseDTO
    {
        [JsonPropertyName("msg")]
        public string? Message { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("storyId")]
        public string? StoryId { get; set; }

        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        public HttpStatusCode? StatusCode { get; internal set; }

        public string? Content { get; internal set; }
    }
}
