using System.Text.Json.Serialization;

namespace Dagnysbageri.api.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ItemNumber { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double PriceKg { get; set; }

        [JsonIgnore]
        public IList<SupplierProduct> SupplierProducts { get; set; }
    }
}