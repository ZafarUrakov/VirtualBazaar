using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private const string meCommand = "Me 👤";

        private async ValueTask<bool> MeAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if (update.Message.Text is meCommand)
                {
                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        message: $"Name: {user.Name}\nPhone number: {user.PhoneNumber}\nAddress: {user.Address}");

                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
