using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary.Contexts;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Projects_VSStudio\Alpha_Asp.NetCourseAssignment\Data\Databases\AlphaDatabase.mdf;Integrated Security=True;Connect Timeout=30");

        return new DataContext(optionsBuilder.Options);
    }
}
