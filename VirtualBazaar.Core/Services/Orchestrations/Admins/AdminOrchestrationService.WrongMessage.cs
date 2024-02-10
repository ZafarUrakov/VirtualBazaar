using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService
    {
        private async ValueTask<bool> WrongMessageAsync(Update update)
        {
            await this.adminTelegramService.SendMessageAsync(
                    userTelegramId: update.Message.Chat.Id,
                    message: "Choose the correct option please ⚠️");

            return true;
        }
    }
}
