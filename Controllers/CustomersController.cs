using Dagnysbageri.api.Interfaces;
using Dagnysbageri.api.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace Dagnysbageri.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    [HttpGet()]

    public async Task<ActionResult> GettAllCustomers()
    {
        var customers = await _unitOfWork.CustomerRepository.List();
        return Ok(new { success = true, data = customers });
    }

    [HttpGet("{id}")]

    public async Task<ActionResult> GetCustomer(int id)
    {
        try
        {
            return Ok(new { success = true, data = await _unitOfWork.CustomerRepository.Get(id) });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpPost()]

    public async Task<ActionResult> Add(CustomerPostViewModel model)
    {
        try
        {
            var result = await _unitOfWork.CustomerRepository.Add(model);
            if (result)
            {
                return StatusCode(201);
            }
            else
            {
                return BadRequest();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}")]

    public async Task<ActionResult> UpdatePerson(int id, UpdateContactPersonViewModel model)
    {
        try
        {
            var result = await _unitOfWork.CustomerRepository.Update(id, model);

            if (result)
            {
                return Ok(new { success = true, message = $"Kontaktperson är uppdaterad." });
            }
            else
            {
                return BadRequest(new { success = false, message = "Kunde inte uppdatera kontaktpersonen!" });
            }
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }
}
