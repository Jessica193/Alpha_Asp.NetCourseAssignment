using BusinessLibrary.Interfaces;
using DomainLibrary.Models;

namespace WebbApp.ViewModels;

public class MembersViewModel
{
    private readonly IMemberService _memberService;

    public MembersViewModel(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public IEnumerable<Member> Members { get; set; } = [];
    public EditMemberViewModel EditMemberForm { get; set; } = new EditMemberViewModel();
    public AddMemberViewModel AddMemberForm { get; set; } = new AddMemberViewModel();


    public async Task<IEnumerable<Member>> PopulateMembersAsync()
    {
        var result = await _memberService.GetMemberssAsync();
        if (result.Succeeded)
        {
            if (result.Result == null) return [];

            Members = result.Result.ToList();
        }

        return [];
    }
}
