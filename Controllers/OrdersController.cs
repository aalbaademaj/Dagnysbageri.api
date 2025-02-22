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
public class OrdersController : ControllerBase
{
    private readonly DataContext _context;
    public OrdersController(DataContext context)
    {
        _context = context;

    }

    [HttpGet()]

    public async Task<ActionResult> List()
    {
        var orders = await _context.SalesOrders
        .Include(so => so.OrderItems)
        .ThenInclude(oi => oi.SalesProduct)
        .Include(so => so.Customer)
        .Select(order => new
        {
            OrderNumber = order.Id,
            order.OrderDate,
            Customer = new
            {
                order.Customer.Id,
                order.Customer.StoreName,
                order.Customer.ContactPerson,
                order.Customer.Email,
                order.Customer.Phone
            },
            Items = order.OrderItems
            .Select(oi => new
            {
                oi.SalesProduct.ProductName,
                oi.SalesProduct.PackQuantity,
                oi.SalesProduct.PiecePrice,
                PackPrice = oi.SalesProduct.PackQuantity * oi.SalesProduct.PiecePrice,
                oi.SalesProduct.Weight,
                oi.Quantity,
                TotalPrice = oi.Quantity * (oi.SalesProduct.PackQuantity * oi.SalesProduct.PiecePrice),
            })
        })
        .ToListAsync();

        return Ok(new { success = true, data = orders });
    }

    [HttpGet("{id}")]

    public async Task<ActionResult> Find(int id)
    {
        try
        {
            var order = await _context.SalesOrders
            .Where(so => so.Id == id)
            .Include(o => o.OrderItems)
            .Select(so => new
            {
                OrderNumber = so.Id,
                so.OrderDate,
                Customer = new
                {
                    so.Customer.Id,
                    so.Customer.StoreName,
                    so.Customer.ContactPerson,
                    so.Customer.Email,
                    so.Customer.Phone
                },
                Items = so.OrderItems
                .Select(oi => new
                {
                    oi.SalesProduct.ProductName,
                    oi.SalesProduct.PackQuantity,
                    oi.SalesProduct.PiecePrice,
                    PackPrice = oi.SalesProduct.PackQuantity * oi.SalesProduct.PiecePrice,
                    oi.SalesProduct.Weight,
                    oi.Quantity,
                    TotalPrice = oi.Quantity * (oi.SalesProduct.PackQuantity * oi.SalesProduct.PiecePrice)
                })
            })
            .SingleOrDefaultAsync();

            if (order is null)
            {
                return NotFound(new { success = false, message = $"Vi kunde inte hitta någon beställning med id {id}" });
            }
            return Ok(new { success = true, data = order });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Ett fel inträffade.", error = ex.Message });
        }
    }

    [HttpGet("date/{date}")]

    public async Task<ActionResult> FindByDate(DateOnly date)
    {
        var result = await _context.SalesOrders
        .Where(s => s.OrderDate == date)
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.SalesProduct)
        .Select(s => new
        {
            OrderNumber = s.Id,
            s.OrderDate,
            Customer = new
            {
                s.Customer.Id,
                s.Customer.StoreName,
                s.Customer.ContactPerson,
                s.Customer.Email,
                s.Customer.Phone
            },
            Items = s.OrderItems
            .Select(oi => new
            {
                oi.SalesProduct.ProductName,
                oi.SalesProduct.PackQuantity,
                oi.SalesProduct.PiecePrice,
                PackPrice = oi.SalesProduct.PackQuantity * oi.SalesProduct.PiecePrice,
                oi.SalesProduct.Weight,
                oi.Quantity,
                TotalPrice = oi.Quantity * (oi.SalesProduct.PackQuantity * oi.SalesProduct.PiecePrice)
            })
        })
        .ToListAsync();

        if (result is null)
        {
            return NotFound(new { success = false, message = $"Vi kunde inte hitta någon beställning med datum {date}" });
        }
        return Ok(new { success = true, data = result });
    }

    [HttpPost]
    public async Task<ActionResult> AddSalesOrder(SalesOrderPostViewModel order)
    {
        // Kontrollera om kunden finns
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == order.CustomerId);
        if (customer == null)
        {
            return NotFound(new { success = false, message = "Kunden existerar inte." });
        }

        var salesOrder = new SalesOrder
        {
            CustomerId = order.CustomerId,
            OrderDate = DateOnly.FromDateTime(DateTime.Now),
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in order.OrderItems)
        {
            var product = await _context.SalesProducts.FirstOrDefaultAsync(p => p.ProductName.ToUpper() == item.SalesProductName.ToUpper());
            if (product == null)
            {
                return NotFound(new { success = false, message = $"Produkten '{item.SalesProductName}' hittades inte." });
            }

            var orderItem = new OrderItem
            {
                SalesProductId = product.Id,
                Quantity = item.Quantity,
                Price = product.PiecePrice * item.Quantity,
            };
        }

        _context.SalesOrders.Add(salesOrder);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(AddSalesOrder), new { id = salesOrder.Id }, new { success = true, orderId = salesOrder.Id });
    }


}
