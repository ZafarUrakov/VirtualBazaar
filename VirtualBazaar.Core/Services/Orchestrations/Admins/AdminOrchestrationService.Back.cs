using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Admins;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService
    {
        private async ValueTask<bool> BackAsync(Update update)
        {
            var admin = this.adminService.RetrieveAllAdmins()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (admin is not null)
            {
                if (update.Message.Text is backCommand
                    && admin.AdminStatus is not AdminStatus.Active)
                {
                    admin.AdminStatus = AdminStatus.Active;
                    await this.adminService.ModifyAdminAsync(admin);
                    ReplyKeyboardMarkup markup = MenuMarkup();

                    await this.adminTelegramService.SendMessageAsync(
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
