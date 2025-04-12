using BusinessLibrary.Interfaces;
using BusinessLibrary.Services;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebbApp.ViewModels;

namespace WebbApp.Controllers;
public class ClientsController(IClientService clientService, IWebHostEnvironment webHostEnvironment) : Controller
{
    private readonly IClientService _clientService = clientService;
    private readonly IWebHostEnvironment _env = webHostEnvironment;


    [HttpPost]
    public async Task<IActionResult> Add(AddClientViewmodel model)
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

        string? imagePath = null;
        if (model.ClientImage != null && model.ClientImage.Length > 0)
        {
            // Cretaing a folder 
            var uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "Clients");
            Directory.CreateDirectory(uploadsFolder);

            // Creating a file path for the uploaded file
            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(model.ClientImage.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            //FileSteram is used to read and write bytes to a file
            using (var fileStream = new FileStream(filePath, FileMode.Create)) //A new empty file is created
            {
                await model.ClientImage.CopyToAsync(fileStream); //The uploaded file is copied to the empty file
            }//The stream closes and the file is saved

            imagePath = Path.Combine("Uploads", "Clients", fileName).Replace("\\", "/");  //ex: "Uploads/Clients/GuidNr.jpg"
        }

        var addClientFormData = model.MapTo<AddClientFormData>();
        addClientFormData.ImagePath = imagePath;
        var result = await _clientService.AddClientAsync(addClientFormData);

        if (result.Succeeded)
            return Ok(new { success = true });

        return Problem("Unable to add client", statusCode: 500);
    }

 


    [HttpPost]
    public async Task<IActionResult> Edit(EditClientViewModel model)
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

        // 1. Hämta klient
        var result = await _clientService.GetClientByIdAsync(model.Id);
        if (!result.Succeeded || result.Result == null)
            return NotFound();

        var client = result.Result;

        // 2. Hantera ny bild
        string? imagePath = null;
        if (model.ClientImage != null && model.ClientImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "Clients");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(model.ClientImage.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create)) 
            {
                await model.ClientImage.CopyToAsync(fileStream);
            }

            imagePath = Path.Combine("Uploads", "Clients", fileName).Replace("\\", "/"); 
        }

        // 3. mappa model till client manuellt för att inte skapa en ny instans av client
        client.ClientName = model.ClientName;
        client.Email = model.Email;
        client.Location = model.Location;
        client.Phone = model.Phone;
        if (imagePath != null)
            client.ImagePath = imagePath;

        // 4. Skicka till service
        var updateResult = await _clientService.EditClientAsync(client);
        if (updateResult.Succeeded)
            return Ok(new { success = true });

        return Problem(updateResult.Error ?? "Unable to edit client", statusCode: updateResult.StatusCode);
    }



    //Genererad av chatgpt för att hämta en klient i samband med att få förifyllt formulär vid editering
    public async Task<IActionResult> GetClient(int id)
    {
        var result = await _clientService.GetClientByIdAsync(id); 
        if (!result.Succeeded || result.Result == null)
            return NotFound();

        return Ok(result.Result); // skickar JSON till JavaScript
    }
}
