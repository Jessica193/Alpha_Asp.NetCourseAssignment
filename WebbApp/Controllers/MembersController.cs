using BusinessLibrary.Interfaces;
using BusinessLibrary.Services;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebbApp.ViewModels;

namespace WebbApp.Controllers;

public class MembersController(IWebHostEnvironment env, IMemberService memberService) : Controller
{
    private readonly IWebHostEnvironment _env = env;
    private readonly IMemberService _memberService = memberService;

    [HttpPost]
    public async Task<IActionResult> Add(AddMemberViewModel model) // ska jag ta emot denna eller MembersViewModel?
    {
        if (!ModelState.IsValid)
        {
            var errorDict = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            //Ta bort sen
            foreach (var kvp in ModelState)
            {
                var key = kvp.Key;
                var modelErrors = kvp.Value?.Errors;
                if (modelErrors?.Count > 0)
                {
                    Debug.WriteLine($"ModelState error for {key}: {string.Join(", ", modelErrors.Select(e => e.ErrorMessage))}");
                }
            }
            //Slut

            return BadRequest(new { success = false, errors = errorDict });
        }

        string? imagePath = null;
        if (model.MemberImage != null && model.MemberImage.Length > 0)
        {
            // Cretaing a folder 
            var uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "Members");
            Directory.CreateDirectory(uploadsFolder);

            // Creating a file path for the uploaded file
            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(model.MemberImage.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            //FileSteram is used to read and write bytes to a file
            using (var fileStream = new FileStream(filePath, FileMode.Create)) //A new empty file is created
            {
                await model.MemberImage.CopyToAsync(fileStream); //The uploaded file is copied to the empty file
            }//The stream closes and the file is saved

            imagePath = Path.Combine("Uploads", "Members", fileName).Replace("\\", "/");  //ex: "Uploads/Clients/GuidNr.jpg"
        }

        var addMemberFormData = model.MapTo<AddMemberFormData>();
        addMemberFormData.ImagePath = imagePath;
        var result = await _memberService.CreateMemberFromAdminAsync(addMemberFormData);

        if (result.Succeeded)
            return Ok(new { success = true });

        return Problem("Unable to add client", statusCode: 500);
    }


    //[HttpPost]
    //public async Task<IActionResult> Edit(EditMemberViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        var errors = ModelState
    //            .Where(x => x.Value?.Errors.Count > 0)
    //            .ToDictionary(
    //                kvp => kvp.Key,
    //                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage)
    //                .ToArray()
    //            );

    //        return BadRequest(new { success = false, errors });
    //    }

    //    // 1. Hämta member
    //    var result = await _memberService.GetMemberByIdAsync(model.Id);
    //    if (!result.Succeeded || result.Result == null)
    //        return NotFound();

    //    var member = result.Result;

    //    // 2. Hantera ny bild
    //    string? imagePath = null;
    //    if (model.MemberImage != null && model.MemberImage.Length > 0)
    //    {
    //        var uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "Members");
    //        Directory.CreateDirectory(uploadsFolder);

    //        var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(model.MemberImage.FileName)}";
    //        var filePath = Path.Combine(uploadsFolder, fileName);

    //        using (var fileStream = new FileStream(filePath, FileMode.Create))
    //        {
    //            await model.MemberImage.CopyToAsync(fileStream);
    //        }

    //        imagePath = Path.Combine("Uploads", "Members", fileName).Replace("\\", "/");
    //    }

    //    // 3. mappa model till member manuellt för att inte skapa en ny instans av member
    //    member.FirstName = model.FirstName;
    //    member.LastName = model.LastName;
    //    member.Email = model.Email;
    //    member.Phone = model.Phone;
    //    member.JobTitle = model.JobTitle;
    //    member.StreetName = model.StreetName;
    //    member.City = model.City;
    //    member.PostalCode = model.PostalCode;
    //    member.DateOfBirth = model.DateOfBirth;
    //    member.se
    //    member.Phone = model.Phone;
    //    if (imagePath != null)
    //        member.ImagePath = imagePath;

    //    // 4. Skicka till service
    //    var updateResult = await _clientService.EditClientAsync(client);
    //    if (updateResult.Succeeded)
    //        return Ok(new { success = true });

    //    return Problem(updateResult.Error ?? "Unable to edit client", statusCode: updateResult.StatusCode);


    //}

    //Genererad av chatgpt för att hämta en klient i samband med att få förifyllt formulär vid editering
    public async Task<IActionResult> GetMember(string id)
    {
        var result = await _memberService.GetMemberByIdAsync(id); // ClientResult<Client>
        if (!result.Succeeded || result.Result == null)
            return NotFound();

        return Ok(result.Result); // skickar JSON till JavaScript
    }
}
