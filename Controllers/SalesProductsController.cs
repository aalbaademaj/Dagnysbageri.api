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
public class SalesProductsController : ControllerBase
{
    private readonly DataContext _context;
    public SalesProductsController(DataContext context)
    {
        _context = context;

    }

    [HttpGet()]

    public async Task<ActionResult> List()
    {
        var salesProducts = await _context.SalesProducts.ToListAsync();
        return Ok(new { success = true, data = salesProducts });
    }

    [HttpGet("{id}")]

    public async Task<ActionResult> Get(int id)
    {
        var salesProduct = await _context.SalesProducts
        .Where(sp => sp.Id == id)
        .SingleOrDefaultAsync();

        if (salesProduct is null)
        {
            return NotFound(new { success = false, message = $"Tyvärr kunde vi inte hitta någon produkt med id {id}" });
        }

        return Ok(new { success = true, data = salesProduct });
    }

    [HttpPost()]

    public async Task<ActionResult> AddSalesProduct(SalesProductPostViewModel model)
    {
        var prod = await _context.SalesProducts.FirstOrDefaultAsync(p => p.ItemNumber == model.ItemNumber);

        if (prod != null)
        {
            return BadRequest(new { success = false, message = $"Produkten existerar redan {0}", model.ProductName });
        }


        var salesProduct = new SalesProduct
        {
            ItemNumber = model.ItemNumber,
            ProductName = model.ProductName,
            PiecePrice = model.PiecePrice,
            Weight = model.Weight,
            PackQuantity = model.PackQuantity,
            ProductionDate = model.ProductionDate,
            ExpireDate = model.ExpireDate
        };

        try
        {
            await _context.SalesProducts.AddAsync(salesProduct);
            await _context.SaveChangesAsync();

            // return Ok(new {success=true, data = product});
            return CreatedAtAction(nameof(Get), new { id = salesProduct.Id }, salesProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

    }

    [HttpPatch("{id}")]

    public async Task<ActionResult> UpdatePrice(int id, [FromQuery] double piecePrice)
    {
        var prod = await _context.SalesProducts.FirstOrDefaultAsync(p => p.Id == id);

        if (prod == null)
        {
            return NotFound(new { success = false, message = $"Produkten som du försöker uppdatera existerar inte längre {0}", id });
        }

        prod.PiecePrice = piecePrice;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return NoContent();
    }
}
