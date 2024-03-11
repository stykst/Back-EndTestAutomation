using System.Text.Json.Serialization;


namespace JsonParser.Models
{

    public class Student
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("courses")]
        public List<Course> Courses { get; set; } = new List<Course>();
    }

    public class Course
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}

