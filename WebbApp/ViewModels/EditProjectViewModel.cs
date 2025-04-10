using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebbApp.ViewModels;

public class EditProjectViewModel
{
    public EditProjectFormData Form { get; set; } = new EditProjectFormData();

    public List<SelectListItem> Members { get; set; } = new List<SelectListItem>();
}


  //public void PopulateClients()
  //  {
  //      var clients = _clientService.GetClients();

  //      Clients = [.. clients.Select(client => new SelectListItem
  //      {
  //          Value = client.Id.ToString(),
  //          Text = client.Name
  //      })];
  //  }
