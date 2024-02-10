using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> DeleteOrderAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if(update.Message.Text.StartsWith("❌")
                    && user.UserStatus is UserStatus.Basket)
                {
                    var orderName = update.Message.Text.Substring(2);

                    var order = this.orderService
                        .RetrieveAllOrders().FirstOrDefault(p => p.Name == orderName);

                    await this.orderService.RemoveOrderAsync(order);

                    var orders = this.orderService.RetrieveAllOrders();
                    var markup = BasketMarkup(orders);
                    string message = CreateMessageForbasket();

                    if (message == "Bakset is empty :(")
                    {
                        var categories = this.categoryService.RetrieveAllCategories();
                        ReplyKeyboardMarkup categorieMarkup = CategoriesMarkup(categories);
                        user.UserStatus = UserStatus.Menu;
                        await this.userService.ModifyUserAsync(user);

                        await this.userTelegramService.SendMessageAsync(
                              userTelegramId: update.Message.Chat.Id,
                              replyMarkup: categorieMarkup,
                              message: message);

                        return true;
                    }

                    await this.userTelegramService.SendMessageAsync(
                       userTelegramId: update.Message.Chat.Id,
                       replyMarkup: markup,
                       message: message);

                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
