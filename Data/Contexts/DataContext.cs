using DataLibrary.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MemberEntity>(options)
{
    public virtual DbSet<MemberAddressEntity> MemberAddresses { get; set; } = null!;
    public virtual DbSet<ProjectEntity> Projects { get; set; } = null!;
    public virtual DbSet<ClientEntity> Clients { get; set; } = null!;
    public virtual DbSet<StatusEntity> Statuses { get; set; } = null!;
}
