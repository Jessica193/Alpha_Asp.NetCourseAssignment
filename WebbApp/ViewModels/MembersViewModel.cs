using BusinessLibrary.Interfaces;
using DomainLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebbApp.ViewModels;

public class MembersViewModel
{
    public IEnumerable<Member> Members { get; set; } = [];
    public EditMemberViewModel EditMemberForm { get; set; } = new EditMemberViewModel();
    public AddMemberViewModel AddMemberForm { get; set; } = new AddMemberViewModel();
    public IEnumerable<SelectListItem> AvailableRoles { get; set; } = [];

}
