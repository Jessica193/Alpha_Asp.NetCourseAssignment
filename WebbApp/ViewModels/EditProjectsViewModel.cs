using DomainLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebbApp.ViewModels;

public class EditProjectsViewModel
{
    public EditProjectsForm Form { get; set; } = new EditProjectsForm();

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
