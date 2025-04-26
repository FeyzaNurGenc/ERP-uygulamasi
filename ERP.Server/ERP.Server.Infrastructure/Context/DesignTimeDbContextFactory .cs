using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ERP.Server.Infrastructure.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())  // Çalışma dizini
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);  // appsettings.json dosyasını ekle

        IConfiguration configuration = builder.Build();
        var connectionString = configuration.GetConnectionString("SqlServer");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);  // Bağlantı dizesini burada kullanıyoruz

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
