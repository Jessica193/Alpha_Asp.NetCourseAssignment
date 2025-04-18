using BusinessLibrary.Interfaces;
using BusinessLibrary.Services;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using WebbApp.ViewModels;

namespace WebbApp.Controllers
{
    public class ProjectsController(IWebHostEnvironment env, IProjectService projectService) : Controller
    {
        private readonly IWebHostEnvironment _env = env;
        private readonly IProjectService _projectService = projectService;

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel model)
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
            if (model.ProjectImage != null && model.ProjectImage.Length > 0)
            {
                // Cretaing a folder 
                var uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "Projects");
                Directory.CreateDirectory(uploadsFolder);

                // Creating a file path for the uploaded file
                var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(model.ProjectImage.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                //FileSteram is used to read and write bytes to a file
                using (var fileStream = new FileStream(filePath, FileMode.Create)) //A new empty file is created
                {
                    await model.ProjectImage.CopyToAsync(fileStream); //The uploaded file is copied to the empty file
                }//The stream closes and the file is saved

                imagePath = Path.Combine("Uploads", "Projects", fileName).Replace("\\", "/");  //ex: "Uploads/Clients/GuidNr.jpg"
            }

            var addProjectFormData = model.MapTo<AddProjectFormData>();
            if (imagePath != null)
                addProjectFormData.ImagePath = imagePath;
            var result = await _projectService.CreateProjectAsync(addProjectFormData);

            if (result.Succeeded)
                return Ok(new { success = true });

            return Problem("Unable to add project", statusCode: 500);


        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
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

            var result = await _projectService.GetOneProjectAsync(model.Id);
            if (!result.Succeeded || result.Result == null)
                return NotFound();

            var project = result.Result;

            string? imagePath = null;
            if (model.ProjectImage != null && model.ProjectImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "Projects");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(model.ProjectImage.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProjectImage.CopyToAsync(fileStream);
                }

                imagePath = Path.Combine("Uploads", "Projects", fileName).Replace("\\", "/");
            }


            var editProjectFormData = model.MapTo<EditProjectFormData>();
            if (imagePath != null)
                editProjectFormData.ImagePath = imagePath;

            var updateResult = await _projectService.EditProjectAsync(editProjectFormData);
            if (updateResult.Succeeded)
                return Ok(new { success = true });

            return Problem(updateResult.Error ?? "Unable to edit project", statusCode: updateResult.StatusCode);


        }

        public async Task<IActionResult> GetProject(int id)
        {
            var result = await _projectService.GetOneProjectAsync(id);
            if (!result.Succeeded || result.Result == null)
                return NotFound();
            var project = result.Result;

            var formData = new EditProjectFormData
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                ImagePath = project.ImagePath,
                ClientId = project.ClientId,
                StatusId = project.StatusId,
                MemberIds = project.Members.Select(m => m.Id).ToList()
            };

            return Ok(formData); // skickar JSON till JavaScript
        }
    }
}
