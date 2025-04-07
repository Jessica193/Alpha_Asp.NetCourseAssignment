using BusinessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebbApp.Controllers;
public class ClientsController : Controller
{
    [HttpPost]
    public IActionResult Add(AddClientForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage)
                    .ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        // Send data to clientservice
        return Ok(new { success = true });
      
        //Tex:
        //var result = await _clientService.AddClientAsync(form);
        //if (result.Success)
        //{
        //    return Ok(new { success = true });
        //}
        //else
        //{
        //    return Problem("An error occurred while adding the client.", statusCode: 500);
        //}
    }


    [HttpPost]
    public IActionResult Edit(EditClientForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage)
                    .ToArray()
                );

            return BadRequest(new { success = false, errors });
        }


        // Send data to clientservice
        return Ok(new { success = true });

        //Tex:
        //var result = await _clientService.UpdateClientAsync(form);
        //if (result.Success)
        //{
        //    return Ok(new { success = true });
        //}
        //else
        //{
        //    return Problem("An error occurred while adding the client.", statusCode: 500);
        //}
    }
}
