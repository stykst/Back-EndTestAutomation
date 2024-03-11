using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlToJson.Models
{


    [XmlRoot("vehicles")]
    public class Vehicles
    {
        [XmlElement("vehicle")]
        public List<Vehicle> VehicleList { get; set; }
    }

    public class Vehicle
    {
        [XmlElement("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("model")]
        public string Model { get; set; } = string.Empty;

        [XmlElement("specs")]
        public string Specs { get; set; } = string.Empty;

        [XmlElement("color")]
        public string Color { get; set; } = string.Empty;

        
        public void Validate()
        {
            if (string.IsNullOrEmpty(Type) ||
                string.IsNullOrEmpty(Model) ||
                string.IsNullOrEmpty(Specs) ||
                string.IsNullOrEmpty(Color))                
            {
                throw new InvalidOperationException("Vehicle object is missing required fields.");
            }
        }
    }
}