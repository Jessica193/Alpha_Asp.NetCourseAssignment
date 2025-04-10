using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DataLibrary.Interfaces;

namespace BusinessLibrary.Services;

public class MemberAddressService(IMemberAddressRepository memberAddressRepository) : IMemberAddressService
{
   private readonly IMemberAddressRepository _memberAddressRepository = memberAddressRepository;

   
}
