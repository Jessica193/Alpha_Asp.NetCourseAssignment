using BusinessLibrary.Models;
using DataLibrary.Interfaces;

namespace BusinessLibrary.Services;

public class MemberAddressService(IMemberAddressRepository memberAddressRepository)
{
   private readonly IMemberAddressRepository _memberAddressRepository = memberAddressRepository;

   
}
