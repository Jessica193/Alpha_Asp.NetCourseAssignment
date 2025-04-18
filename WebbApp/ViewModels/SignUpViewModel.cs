﻿using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name", Prompt = "Your first name")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name", Prompt = "Your last name")]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Your email adress")]
    [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Invalid password")]
    public string Password { get; set; } = null!;


    [Required (ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Required")]
    public bool TermsAccepted { get; set; }
}

