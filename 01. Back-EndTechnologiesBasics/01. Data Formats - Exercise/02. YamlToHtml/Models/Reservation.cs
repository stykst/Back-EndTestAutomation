using YamlDotNet.Serialization;

namespace YamlToHtml.Models
{
    public class Reservation
    {
        [YamlMember(Alias = "reservation_id", ApplyNamingConventions = false)]
        public int? ReservationId { get; set; }

        [YamlMember(Alias = "guest_name", ApplyNamingConventions = false)]
        public string? GuestName { get; set; }

        [YamlMember(Alias = "services", ApplyNamingConventions = false)]
        public List<Service> Services { get; set; } = new List<Service>();
    }

    public class Service
    {
        [YamlMember(Alias = "type", ApplyNamingConventions = false)]
        public string? Type { get; set; }

        [YamlMember(Alias = "date", ApplyNamingConventions = false)]
        public string? Date { get; set; }

        [YamlMember(Alias = "time", ApplyNamingConventions = false)]
        public string? Time { get; set; }
    }
}
