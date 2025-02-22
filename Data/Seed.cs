using System.Text.Json;
using Dagnysbageri.api.Entities;

namespace Dagnysbageri.api.Data.Migrations;

public class Seed
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public static async Task LoadProducts(DataContext context)
    {

        if (context.Products.Any()) return;

        var json = File.ReadAllText("Data/json/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(json, options);

        if (products is not null && products.Count > 0)
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadSuppliers(DataContext context)
    {
        if (context.Suppliers.Any()) return;

        // Annars , hämtar vi data ur json filen...
        var json = File.ReadAllText("Data/json/suppliers.json");
        var suppliers = JsonSerializer.Deserialize<List<Supplier>>(json, options);

        if (suppliers is not null && suppliers.Count > 0)
        {
            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadSupplierProducts(DataContext context)
    {

        if (context.SupplierProducts.Any()) return;

        // Annars , hämtar vi data ur json filen...
        var json = File.ReadAllText("Data/json/supplierproducts.json");
        var suppliersproducts = JsonSerializer.Deserialize<List<SupplierProduct>>(json, options);

        if (suppliersproducts is not null && suppliersproducts.Count > 0)
        {
            await context.SupplierProducts.AddRangeAsync(suppliersproducts);
            await context.SaveChangesAsync();
        }
    }

    public static async Task LoadCustomers(DataContext context)
    {

        if (context.Customers.Any()) return;

        // Annars , hämtar vi data ur json filen...
        var json = File.ReadAllText("Data/json/customers.json");
        var customers = JsonSerializer.Deserialize<List<Customer>>(json, options);

        if (customers is not null && customers.Count > 0)
        {
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadSalesProducts(DataContext context)
    {
        if (context.SalesProducts.Any()) return;

        // Annars , hämtar vi data ur json filen...
        var json = File.ReadAllText("Data/json/salesProducts.json");
        var salesProducts = JsonSerializer.Deserialize<List<SalesProduct>>(json, options);

        if (salesProducts is not null && salesProducts.Count > 0)
        {
            await context.SalesProducts.AddRangeAsync(salesProducts);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadSalesOrders(DataContext context)
    {
        if (context.SalesOrders.Any()) return;

        // Annars , hämtar vi data ur json filen...
        var json = File.ReadAllText("Data/json/salesOrders.json");
        var salesOrders = JsonSerializer.Deserialize<List<SalesOrder>>(json, options);

        if (salesOrders is not null && salesOrders.Count > 0)
        {
            await context.SalesOrders.AddRangeAsync(salesOrders);
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadOrderItems(DataContext context)
    {
        if (context.OrderItems.Any()) return;

        // Annars , hämtar vi data ur json filen...
        var json = File.ReadAllText("Data/json/orderItems.json");
        var orderItems = JsonSerializer.Deserialize<List<OrderItem>>(json, options);

        if (orderItems is not null && orderItems.Count > 0)
        {
            await context.OrderItems.AddRangeAsync(orderItems);
            await context.SaveChangesAsync();
        }
    }


    public static async Task LoadAddressTypes(DataContext context)
    {
        if (context.AddressTypes.Any()) return;

        var json = await File.ReadAllTextAsync("Data/json/addressTypes.json");
        var types = JsonSerializer.Deserialize<List<AddressType>>(json, options);

        if (types is not null && types.Count > 0)
        {
            await context.AddressTypes.AddRangeAsync(types);
            await context.SaveChangesAsync();
        }
    }
}
