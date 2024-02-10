using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService
    {
        private const string meCommand = "Me 👤";

        private async ValueTask<bool> MeAsync(Update update)
        {
            var admin = this.adminService.RetrieveAllAdmins()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (admin is not null)
            {
                if (update.Message.Text is meCommand)
                {
                    await this.adminTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        message: $"Organization Name: {admin.OrganizationName}\nPhone number: {admin.PhoneNumber}\nOrganization Address: {admin.Address}");

                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
