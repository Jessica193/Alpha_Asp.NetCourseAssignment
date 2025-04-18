﻿using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DataLibrary.Repositories;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
namespace BusinessLibrary.Services;

public class MemberService(IMemberRepository memberRepository, UserManager<MemberEntity> userManager, RoleManager<IdentityRole> roleManager) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    public async Task<MemberResult> CreateMemberFromSignUpAsync(SignUpFormData form, string roleName = "User")
    {
        if (form == null)
            return new MemberResult{Succeeded = false, StatusCode = 400, Error = "Form is null"};

        var existResult = await _memberRepository.ExistsAsync(x => x.Email == form.Email);
        if (existResult.Succeeded)
            return new MemberResult{Succeeded = false, StatusCode = 409, Error = "Member with same email already exists" };

        try
        {
            var memberEntity = form.MapTo<MemberEntity>();
            memberEntity.UserName = form.Email;

            var result = await _userManager.CreateAsync(memberEntity, form.Password);
            if (result.Succeeded)
            {
                var addToRoleResult = await AddMemberToRoleAsync(memberEntity.Id, roleName);
                return addToRoleResult.Succeeded 
                    ? new MemberResult { Succeeded = true, StatusCode = 201 }
                    : new MemberResult { Succeeded = false, StatusCode = 201, Error = "Member created but not added to role" };
            }
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Member not created" };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
      
    }

    public async Task<MemberResult> CreateMemberFromAdminAsync(AddMemberFormData form)
    {
        if (form == null)
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Form is null" };

        var existResult = await _memberRepository.ExistsAsync(x => x.Email == form.Email);
        if (existResult.Succeeded)
            return new MemberResult { Succeeded = false, StatusCode = 409, Error = "Member with same email already exists" };

        try
        {
            var memberEntity = form.MapTo<MemberEntity>();
            memberEntity.UserName = form.Email;

            var result = await _userManager.CreateAsync(memberEntity, "Bytmig123!");
            if (!result.Succeeded)
                return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Member not created" };

            var memberAdress = form.MapTo<MemberAddressEntity>();
            memberAdress.UserId = memberEntity.Id;
            memberEntity.Address = memberAdress;
            var updateResult = await _userManager.UpdateAsync(memberEntity);
            if (!updateResult.Succeeded)
                return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Member created but no address added" };

            var roleResult = await AddMemberToRoleAsync(memberEntity.Id, form.SelectedRole);
            return roleResult.Succeeded
                ? new MemberResult { Succeeded = true, StatusCode = 201 }
                : new MemberResult { Succeeded = false, StatusCode = 201, Error = "Member created but no role added" };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }


    public async Task<MemberResult<IEnumerable<Member>>> GetMembersAsync()
    {
        var result = await _memberRepository.GetAllAsync(
            includes: include => include.Address!
        );

        return result.MapTo<MemberResult<IEnumerable<Member>>>();
    }


    public async Task<MemberResult<Member>> GetMemberByIdAsync(string id)
    {
        var result = await _memberRepository.GetOneAsync(x => x.Id == id, x => x.Address!);
        if (!result.Succeeded || result.Result == null)
            return new MemberResult<Member> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };

        var member = result.Result;

        var entity = await _userManager.FindByIdAsync(id);
        if (entity == null)
            return new MemberResult<Member> { Succeeded = false, StatusCode = 404, Error = "Member entity not found" };

        var roles = await _userManager.GetRolesAsync(entity);
        member.SelectedRole = roles.FirstOrDefault() ?? "";

        return new MemberResult<Member>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = member
        };
    }

    public async Task<MemberResult> EditMemberAsync(Member member)
    {
        if (member == null)
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Member is null" };

        try
        {
            var entityResult = await _memberRepository.GetOneEntityAsync(x => x.Id == member.Id);

            if (!entityResult.Succeeded || entityResult.Result == null)
                return new MemberResult { Succeeded = false, StatusCode = 404, Error = "Member not found" };

            var existingEntity = entityResult.Result;

            existingEntity.FirstName = member.FirstName;
            existingEntity.LastName = member.LastName;
            existingEntity.Email = member.Email;
            existingEntity.PhoneNumber = member.PhoneNumber;
            existingEntity.JobTitle = member.JobTitle;
            existingEntity.DateOfBirth = member.DateOfBirth;
            existingEntity.NormalizedEmail = member.Email.ToUpper();
            existingEntity.NormalizedUserName = member.Email.ToUpper();
            existingEntity.UserName = member.Email;
            
            if (member.ImagePath != null)
                existingEntity.ImagePath = member.ImagePath;

            // Adress (om du hanterar det)
            if (existingEntity.Address != null)
            {
                existingEntity.Address.StreetName = member.Address?.StreetName;
                existingEntity.Address.City = member.Address?.City;
                existingEntity.Address.PostalCode = member.Address?.PostalCode;
            }
            else if (member.Address != null)
            {
                existingEntity.Address = new MemberAddressEntity
                {
                    UserId = member.Id,
                    StreetName = member.Address.StreetName,
                    City = member.Address.City,
                    PostalCode = member.Address.PostalCode
                };
            }

            // 3. Uppdatera användarens roll
            if (!string.IsNullOrWhiteSpace(member.SelectedRole))
            {
                var currentRoles = await _userManager.GetRolesAsync(existingEntity);

                if (currentRoles.Any())
                    await _userManager.RemoveFromRolesAsync(existingEntity, currentRoles);

                await _userManager.AddToRoleAsync(existingEntity, member.SelectedRole);
            }


            var updateResult = await _userManager.UpdateAsync(existingEntity);
            return updateResult.Succeeded
                ? new MemberResult { Succeeded = true, StatusCode = 200 }
                : new MemberResult { Succeeded = false, StatusCode = 500, Error = "Member not updated" };

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }


    public async Task<MemberResult> AddMemberToRoleAsync(string memberId, string roleName)
    {
        var role = await _roleManager.RoleExistsAsync(roleName);
        if (!role)
            return new MemberResult{Succeeded = false,StatusCode = 404,Error = "Role does not exist"};

        var member = await _userManager.FindByIdAsync(memberId);
        if (member == null)
            return new MemberResult{Succeeded = false,StatusCode = 404,Error = "Member not found"};

        var result = await _userManager.AddToRoleAsync(member, roleName);
        if (result.Succeeded)
            return new MemberResult{Succeeded = true,StatusCode = 200,};

        return new MemberResult {Succeeded = false,StatusCode = 500,Error = "Unable to add member to role"};

    }


    public async Task<bool> MemberExists(string email)
    {
        if (await _userManager.FindByEmailAsync(email) != null)
            return true;


        return false;

    }

}
