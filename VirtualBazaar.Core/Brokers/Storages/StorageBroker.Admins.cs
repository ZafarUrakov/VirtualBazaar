using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtualBazaar.Core.Models.Foundations.Admins;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Admin> Admins { get; set; }

        public async ValueTask<Admin> InsertAdminAsync(Admin admin) =>
            await InsertAsync(admin);
        public IQueryable<Admin> SelectAllAdmins() =>
            SelectAll<Admin>();
        public async ValueTask<Admin> UpdateAdminAsync(Admin admin) =>
            await UpdateAsync(admin);
    }
}
