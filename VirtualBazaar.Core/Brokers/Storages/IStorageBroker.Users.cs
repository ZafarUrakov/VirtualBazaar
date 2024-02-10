using System.Linq;
using System.Threading.Tasks;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<User> InsertUserAsync(User user);
        IQueryable<User> SelectAllUsers();
        ValueTask<User> UpdateUserAsync(User user);
    }
}
