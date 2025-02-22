using Dagnysbageri.api.Data.Migrations;
using Dagnysbageri.api.Entities;
using Dagnysbageri.api.Interfaces;
using Dagnysbageri.api.ViewModels;
using Dagnysbageri.api.ViewModels.Address;
using Dagnysbageri.api.ViewModels.Customer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Dagnysbageri.api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _context;
    private readonly IAddressRepository _repo;
    public CustomerRepository(DataContext context, IAddressRepository repo)
    {
        _repo = repo;
        _context = context;

    }
    public async Task<bool> Add(CustomerPostViewModel model)
    {
        try
        {
            var customerexists = await _context.Customers
                .FirstOrDefaultAsync(c => c.StoreName.ToLower().Trim() == model.StoreName.ToLower().Trim());

            if (customerexists is not null)
            {
                throw new Exception("Kunden finns redan!!");
            }

            var customer = new Customer
            {
                StoreName = model.StoreName,
                Phone = model.Phone,
                Email = model.Email,
                ContactPerson = model.ContactPerson,
                CustomerAddresses = new List<CustomerAddress>()
            };

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            foreach (var addressModel in model.Addresses)
            {
                var address = await _repo.Add(addressModel);

                customer.CustomerAddresses.Add(new CustomerAddress
                {
                    CustomerId = customer.Id,
                    AddressId = address.Id
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Fel inträff: {ex.Message}");
        }
    }

    public async Task<CustomerViewModel> Get(int id)
    {
        try
        {
            var customer = await _context.Customers
              .Where(c => c.Id == id)
              .Include(c => c.CustomerAddresses)
                .ThenInclude(c => c.Address)
                .ThenInclude(c => c.PostalAddress)
              .Include(c => c.CustomerAddresses)
                .ThenInclude(c => c.Address)
                .ThenInclude(c => c.AddressType)
                .Include(c => c.SalesOrders)
                .ThenInclude(so => so.OrderItems)
                .ThenInclude(so => so.SalesProduct)
              .SingleOrDefaultAsync();

            if (customer is null)
            {
                throw new Exception($"Det finns ingen kund med id {id}");
            }

            var view = new CustomerViewModel
            {
                Id = customer.Id,
                StoreName = customer.StoreName,
                Phone = customer.Phone,
                Email = customer.Email,
                ContactPerson = customer.ContactPerson
            };

            var addresses = customer.CustomerAddresses.Select(c => new AddressViewModel
            {
                AddressLine = c.Address.AddressLine,
                PostalCode = c.Address.PostalAddress.PostalCode,
                City = c.Address.PostalAddress.City,
                AddressType = c.Address.AddressType.Value
            });

            view.Addresses = [.. addresses];

            var orders = customer.SalesOrders.Select(o => new SalesOrderViewModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
                {
                    SalesProductName = oi.SalesProduct.ProductName,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }).ToList();

            view.SalesOrders = orders;

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<CustomersViewModel>> List()
    {
        var response = await _context.Customers.ToListAsync();
        var customers = response.Select(c => new CustomersViewModel
        {
            Id = c.Id,
            StoreName = c.StoreName,
            Phone = c.Phone,
            Email = c.Email,
            ContactPerson = c.ContactPerson
        });

        return [.. customers];
    }

    public async Task<bool> Update(int id, UpdateContactPersonViewModel model)
    {
        try
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);

            if (customer is null)
            {
                throw new Exception($"Det finns ingen kund med id {id}");
            }

            customer.ContactPerson = model.ContactPerson;

            _context.Customers.Update(customer);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }

    }
}
