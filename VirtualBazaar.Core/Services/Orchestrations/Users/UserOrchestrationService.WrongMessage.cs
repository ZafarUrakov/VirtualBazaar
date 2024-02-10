using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> WrongMessageAsync(Update update)
        {
            await this.userTelegramService.SendMessageAsync(
                    userTelegramId: update.Message.Chat.Id,
                    message: "Choose the correct option please ⚠️");

            return true;
        }
    }
}
