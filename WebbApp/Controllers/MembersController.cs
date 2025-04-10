using BusinessLibrary.Interfaces;
using BusinessLibrary.Services;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using WebbApp.ViewModels;

namespace WebbApp.Controllers;

public class MembersController(IWebHostEnvironment env, IMemberService memberService) : Controller
{
    private readonly IWebHostEnvironment _env = env;
    private readonly IMemberService _memberService = memberService;

    [HttpPost]
    public async Task<IActionResult> Add(AddMemberViewModel model)
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


    [HttpPost]
    public IActionResult Edit(EditMemberViewModel form)
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


   
        return Ok(new { success = true });

    
    }
}
