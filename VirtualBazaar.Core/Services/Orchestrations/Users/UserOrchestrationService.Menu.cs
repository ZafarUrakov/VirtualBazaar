using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private const string menuCommant = "Menu 🛍";

        private async ValueTask<bool> MenuAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (update.Message.Text is menuCommant)
            {
                var categories = this.categoryService.RetrieveAllCategories();
                string message = "Welcome to Menu, choose what you like 🍓:";

                if (categories == null || !categories.Any())
                {
                    message = "Empty :(";
                }

                ReplyKeyboardMarkup markup = CategoriesMarkup(categories);
                user.UserStatus = UserStatus.Menu;
                await this.userService.ModifyUserAsync(user);

                await this.userTelegramService.SendMessageAsync(
                           userTelegramId: update.Message.Chat.Id,
                           replyMarkup: markup,
                           message: message);

                return true;
            }

            return false;
        }
    }
}
