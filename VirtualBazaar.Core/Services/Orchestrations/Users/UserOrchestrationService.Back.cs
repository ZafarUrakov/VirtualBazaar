using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> BackAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if (update.Message.Text is backCommand
                    && user.UserStatus is not UserStatus.Active)
                {
                    user.UserStatus = UserStatus.Active;
                    await this.userService.ModifyUserAsync(user);
                    ReplyKeyboardMarkup markup = MenuMarkup();

                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        replyMarkup: markup,
                        message: "Good, choose the options please 👀:");

                    return true;
                }
            }

            return false;
        }
    }
}
