using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dagnysbageri.api.Data.Migrations;
using Dagnysbageri.api.Entities;
using Dagnysbageri.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dagnysbageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly DataContext _context;
    public ProductsController(DataContext context)
    {
        _context = context;

    }

    [HttpGet()]

    public async Task<ActionResult> ListProducts()
    {
        var products = await _context.Products
        .Include(sp => sp.SupplierProducts)
        .ThenInclude(s => s.Supplier)
        .ThenInclude(s => s.SupplierAddresses)
        .Select(p => new
        {
            p.ProductId,
            p.ItemNumber,
            p.ProductName,
            p.Description,
            p.PriceKg,
            Suppliers = p.SupplierProducts
            .Select(sp => new
            {
                sp.Supplier.Name,
                sp.Supplier.ContactPerson,
                sp.Supplier.Phone,
                sp.Supplier.Email,
                sp.Price,
                Addresses = sp.Supplier.SupplierAddresses
                .Select(sa => new
                {
                    sa.Address.AddressLine,
                    sa.Address.PostalAddress.City,
                    sa.Address.PostalAddress.PostalCode,
                    sa.Address.AddressType.Value
                })
            })
        }).ToListAsync();

        return Ok(new { success = true, statuscode = 200, data = products });
    }

    [HttpGet("{name}")]

    public async Task<ActionResult> FindProduct(string name)
    {
        var products = await _context.Products
        .Include(sp => sp.SupplierProducts)
        .ThenInclude(s => s.Supplier)
        .Where(p => p.ProductName.ToUpper() == name.ToUpper())
        .Select(p => new
        {
            p.ProductId,
            p.ItemNumber,
            p.ProductName,
            p.Description,
            p.PriceKg,
            Suppliers = p.SupplierProducts
            .Select(sp => new
            {
                sp.Supplier.Name,
                sp.Supplier.ContactPerson,
                sp.Supplier.Phone,
                sp.Supplier.Email,
                sp.Price,
                Addresses = sp.Supplier.SupplierAddresses
                .Select(sa => new
                {
                    sa.Address.AddressLine,
                    sa.Address.PostalAddress.City,
                    sa.Address.PostalAddress.PostalCode,
                    sa.Address.AddressType.Value
                })
            })
        }).ToListAsync();

        return Ok(new { success = true, statuscode = 200, data = products });
    }


    [HttpPost("{supplierId}")]

    public async Task<ActionResult> AddProduct(int supplierId, ProductPostViewModel model)
    {
        var prod = await _context.Products.FirstOrDefaultAsync(p => p.ItemNumber == model.ItemNumber);

        if (prod != null)
        {
            return BadRequest(new { success = false, message = $"Produkten som du försöker lägga till finns redan {0}", model.ProductName });
        }

        var newProduct = new Product
        {
            ItemNumber = model.ItemNumber,
            ProductName = model.ProductName,
            Description = model.Description,
            PriceKg = model.PriceKg
        };

        var supplierProduct = new SupplierProduct
        {
            SupplierId = supplierId,
            Product = newProduct,
            Price = model.PriceKg
        };

        try
        {
            await _context.Products.AddAsync(newProduct);
            await _context.SupplierProducts.AddAsync(supplierProduct);
            await _context.SaveChangesAsync();



            return CreatedAtAction(nameof(FindProduct), new { name = newProduct.ProductName }, newProduct);
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
    }

    [HttpPatch("{productId}/suppliers/{supplierId}/price")]

    public async Task<ActionResult> UpdateProductPrice(int productId, int supplierId, UpdatePriceViewModel model)
    {
        var supplierProduct = await _context.SupplierProducts
        .FirstOrDefaultAsync(sp => sp.ProductId == model.ProductId && sp.SupplierId == model.SupplierId);

        if (supplierProduct == null)
        {
            return NotFound(new { success = false, message = $"Hittar inget!!" });
        }
        supplierProduct.Price = model.newPrice;

        try
        {
            _context.SupplierProducts.Update(supplierProduct);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Prisen har uppdaterats", newPrice = supplierProduct.Price });
        }
        catch (Exception ex)
        {

            return StatusCode(500, ex.Message);
        }
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var toDelete = await _context.Products.SingleOrDefaultAsync(p => p.ProductId == id);
        _context.Products.Remove(toDelete);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
