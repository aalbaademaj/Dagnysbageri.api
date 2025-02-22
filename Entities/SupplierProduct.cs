namespace Dagnysbageri.api.Entities
{
    public class SupplierProduct
    {
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }

        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
    }
}