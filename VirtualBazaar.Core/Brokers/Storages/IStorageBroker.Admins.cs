using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Models.Foundations.Admins;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Admin> InsertAdminAsync(Admin admin);
        IQueryable<Admin> SelectAllAdmins();
        ValueTask<Admin> UpdateAdminAsync(Admin admin);
    }
}
