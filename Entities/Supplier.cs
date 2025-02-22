using System.Text.Json.Serialization;

namespace Dagnysbageri.api.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public IList<SupplierProduct> SupplierProducts { get; set; }
        public IList<SupplierAddress> SupplierAddresses { get; set; }

    }
}