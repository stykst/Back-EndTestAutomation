using YamlDotNet.Serialization;

namespace YamlToHtml.Models
{
    public class Order
    {
        [YamlMember(Alias = "order_id", ApplyNamingConventions = false)]
        public int OrderId { get; set; }

        [YamlMember(Alias = "customer", ApplyNamingConventions = false)]
        public string Customer { get; set; }

        [YamlMember(Alias = "item", ApplyNamingConventions = false)]
        public string Item { get; set; }

        [YamlMember(Alias = "quantity", ApplyNamingConventions = false)]
        public int Quantity { get; set; }

        [YamlMember(Alias = "total_amount", ApplyNamingConventions = false)]
        public decimal TotalAmount { get; set; }
    }
}
