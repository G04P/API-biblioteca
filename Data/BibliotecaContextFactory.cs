using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class BibliotecaContextFactory : IDesignTimeDbContextFactory<BibliotecaContext>
{
    public BibliotecaContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BibliotecaContext>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new BibliotecaContext(optionsBuilder.Options);
    }
}
