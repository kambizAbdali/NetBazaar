using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NetBazaar.Infrastructure.Data;

namespace NetBazaar.Persistence
{
    public class NetBazaarDbContextFactory : IDesignTimeDbContextFactory<NetBazaarDbContext>
    {
        public NetBazaarDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NetBazaarDbContext>();

            // کانکشن استرینگ رو اینجا ست کن
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=NetBazaar;Integrated Security=True;TrustServerCertificate=true;");

            return new NetBazaarDbContext(optionsBuilder.Options);
        }
    }
}
