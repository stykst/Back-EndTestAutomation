using System.Xml.Serialization;


namespace XmlToJson.Models
{
    [XmlRoot("devices")]
    public class Devices
    {
        [XmlElement("device")]
        public List<Device> DeviceList { get; set; }
    }

    public class Device
    {
        [XmlElement("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("brand")]
        public string Brand { get; set; } = string.Empty;

        [XmlElement("specs")]
        public string Specs { get; set; } = string.Empty;

        [XmlElement("price")]
        public int Price { get; set; } 

        public void Validate()
        {
            if (string.IsNullOrEmpty(Type) ||
                string.IsNullOrEmpty(Brand) ||
                string.IsNullOrEmpty(Specs)) 
            {
                throw new InvalidOperationException("Device object is missing required fields.");
            }
        }
    }
}