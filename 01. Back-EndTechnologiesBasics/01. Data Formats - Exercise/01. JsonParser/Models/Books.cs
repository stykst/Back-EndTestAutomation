using System.Text.Json.Serialization;

namespace JsonParser.Models
{
    class Book
    {
        [JsonPropertyName("title")]
        public required string Title { get; set; }

        [JsonPropertyName("author")]
        public required string Author { get; set; }

        [JsonPropertyName("released")]
        public int Released { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("ISBN")]
        public required string ISBN { get; set; }
    }
}
