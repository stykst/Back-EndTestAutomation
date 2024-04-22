using System.Net;
using System.Text.Json.Serialization;

namespace Foody.RestFulApi.Test.DTOs
{
    public class ApiResponseDTO
    {
        [JsonPropertyName("msg")]
        public string? Message { get; set; }

        [JsonPropertyName("id")]
        public string? FoodId { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        public HttpStatusCode? StatusCode { get; internal set; }

        public string? Content { get; internal set; }
    }
}
