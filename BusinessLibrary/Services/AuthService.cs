using DataLibrary.Entities;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLibrary.Services;

public class AuthService(UserManager<MemberEntity> userManager, SignInManager<MemberEntity> signInManager)
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;


    public async Task<bool> CreateAccountAsync(CreateAccountForm form)
    {
        var existingUser = await _userManager.FindByEmailAsync(form.Email);
        if (existingUser != null)
        {
            return false;
        }

        var memberEntity = new MemberEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            UserName = form.Email
        };

        var result = await _userManager.CreateAsync(memberEntity, form.Password);

        if (result.Succeeded)
        {
            var addressEntity = new MemberAddressEntity
            {
                UserId = memberEntity.Id, 
                StreetName = form.StreetName,
                City = form.City,
                PostalCode = form.PostalCode
            };

            //_context.MemberAddresses.Add(addressEntity);
            //await _context.SaveChangesAsync();

            return true;
        }

        return false;
    
    }

    public async Task<bool> LogInAsync(MemberLogInForm form)
    {
        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, false);
        return result.Succeeded;
    }

    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> UserExists(string email)
    {
        if (await _userManager.FindByEmailAsync(email) != null)
        {
            return true;
        }

        return false;

    }
}
