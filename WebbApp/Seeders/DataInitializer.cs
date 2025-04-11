using DataLibrary.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebbApp.Seeders;

public static class DataInitializer
{

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedRoleAsync(roleManager);

        var userManager = services.GetRequiredService<UserManager<MemberEntity>>();
        await SeedDefaultAdminAsync(userManager);
    }


    public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["User", "Admin"];

        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
            
        }
    }

    public static async Task SeedDefaultAdminAsync(UserManager<MemberEntity> userManager)
    {
        var adminEmail = "Admin@domain.com";
        var adminPassword = "Bytmig123!";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new MemberEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "Adminsson",
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "Admin");
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating admin user: {error.Description}");
                }
            }

        }
     

    }

}
