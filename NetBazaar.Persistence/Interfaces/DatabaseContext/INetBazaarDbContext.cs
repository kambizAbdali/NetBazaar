using Microsoft.EntityFrameworkCore;
using NetBazaar.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace NetBazaar.Persistence.Interfaces.DatabaseContext
{
    public interface INetBazaarDbContext
    {
        public int SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess);
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
