using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private const string contactUsCommand = "Contact us ☎️";

        private async ValueTask<bool> ContactUsAsync(Update update)
        { 
            if(update.Message.Text is contactUsCommand)
            {
                var admin = this.adminService.RetrieveAllAdmins()
                    .FirstOrDefault(a => a.TelegramId == update.Message.Chat.Id);

                if(admin is not null)
                {
                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        message: $"{admin.OrganizationName}\nNumber: {admin.PhoneNumber}\nAddress: {admin.Address}");

                    return true;
                }
                else
                {
                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        message: $"Under developing...");

                    return true;
                }

            }

            return false;
        }
    }
}
