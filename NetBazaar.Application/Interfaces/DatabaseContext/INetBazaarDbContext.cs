using Microsoft.EntityFrameworkCore;
using NetBazaar.Domain.Entities;

namespace NetBazaar.Application.Interfaces.DatabaseContext
{
    public interface INetBazaarDbContext
    {
        public DbSet<User> Users { get; set; }

        public int SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess);
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
