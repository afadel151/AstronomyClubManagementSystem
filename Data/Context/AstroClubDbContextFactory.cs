// Data/Context/AstroClubDbContextFactory.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data.Context;

public class AstroClubDbContextFactory
    : IDesignTimeDbContextFactory<AstroClubDbContext>
{
    public AstroClubDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<AstroClubDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;" +
            "Database=AstroClubDb;" +
            "User Id=sa;" +
            "Password=AstroClub2025;" +
            "TrustServerCertificate=True");

        return new AstroClubDbContext(optionsBuilder.Options);
    }
}