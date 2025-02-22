using Dagnysbageri.api.Data;
using Dagnysbageri.api.Data.Migrations;
using Dagnysbageri.api.Interfaces;
using Dagnysbageri.api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(9, 2, 0));
builder.Services.AddDbContext<DataContext>
(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
    //options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
});

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.LoadProducts(context);
    await Seed.LoadSuppliers(context);
    await Seed.LoadCustomers(context);
    await Seed.LoadSalesProducts(context);
    await Seed.LoadAddressTypes(context);
    await Seed.LoadSalesOrders(context);
    await Seed.LoadOrderItems(context);
    await Seed.LoadSupplierProducts(context);
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}

app.MapControllers();

app.Run();
