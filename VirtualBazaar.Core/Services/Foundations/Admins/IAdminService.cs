using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Models.Foundations.Admins;

namespace VirtualBazaar.Core.Services.Foundations.Admins
{
    public interface IAdminService
    {
        ValueTask<Admin> AddAdminAsync(Admin admin);
        IQueryable<Admin> RetrieveAllAdmins();
        ValueTask<Admin> ModifyAdminAsync(Admin admin);
    }
}
