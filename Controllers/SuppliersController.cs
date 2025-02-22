using Dagnysbageri.api.Data.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dagnysbageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly DataContext _context;
    public SuppliersController(DataContext context)
    {
        _context = context;

    }

    [HttpGet()]

    public async Task<ActionResult> List()
    {
        var suppliers = await _context.Suppliers.ToListAsync();

        if (suppliers is null)
        {
            return NotFound(new { success = false, message = $"Vi hittar ingen leverantör!!" });
        }
        return Ok(new { success = true, data = suppliers });
    }


    [HttpGet("{name}")]
    public async Task<ActionResult> ListSuppliers(string name)
    {
        var suppliers = await _context.Suppliers
        .Include(sp => sp.SupplierProducts)
        .ThenInclude(p => p.Product)
        .Include(sa => sa.SupplierAddresses)
        .ThenInclude(sa => sa.Address)
        .Where(s => s.Name.ToUpper() == name.ToUpper())
        .Select(s => new
        {
            s.SupplierId,
            Supplier = s.Name,
            s.ContactPerson,
            s.Phone,
            s.Email,
            Products = s.SupplierProducts
            .Select(sp => new
            {
                sp.Product.ProductName,
                sp.Product.Description,
                sp.Price
            }),
            Addresses = s.SupplierAddresses
            .Select(sa => new
            {
                sa.Address.AddressLine,
                sa.Address.PostalAddress.City,
                sa.Address.PostalAddress.PostalCode,
                sa.Address.AddressType.Value
            })
        }).ToListAsync();

        return Ok(new { success = true, StatusCode = 200, data = suppliers });
    }
}
