using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    public class ProjectsController : Controller
    {
        [HttpPost]
        public IActionResult Add(AddProjectViewModel form)
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
        public IActionResult Edit(EditProjectFormData form)
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
}
