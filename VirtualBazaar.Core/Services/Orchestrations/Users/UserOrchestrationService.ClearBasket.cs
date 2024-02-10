using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> ClearBasketAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if (await CheckIfBackCommandOnChooseBasket(update, user))
                    return true;

                if (update.Message.Text is clearBasketCommand
                    && user.UserStatus is UserStatus.Basket)
                {
                    var orders = this.orderService.RetrieveAllOrders();

                    foreach(var order in orders)
                    {
                        await this.orderService.RemoveOrderAsync(order);
                    }

                    var categories = this.categoryService.RetrieveAllCategories();
                    ReplyKeyboardMarkup markup = CategoriesMarkup(categories);
                    user.UserStatus = UserStatus.Menu;
                    await this.userService.ModifyUserAsync(user);

                    await this.userTelegramService.SendMessageAsync(
                               userTelegramId: update.Message.Chat.Id,
                               replyMarkup: markup,
                               message: "Clear, let's continue 😃:");

                    return true;
                }

                return false;
            }

            return false;
        }

        private async Task<bool> CheckIfBackCommandOnChooseBasket(
            Update update, 
            Models.Foundations.Users.User user)
        {
            if (update.Message.Text is backCommand
               && user.UserStatus is UserStatus.Basket)
            {
                var categories = this.categoryService.RetrieveAllCategories();
                ReplyKeyboardMarkup markup = CategoriesMarkup(categories);
                user.UserStatus = UserStatus.Menu;
                await this.userService.ModifyUserAsync(user);

                await this.userTelegramService.SendMessageAsync(
                           userTelegramId: update.Message.Chat.Id,
                           replyMarkup: markup,
                           message: "Let's continue 😃:");

                return true;
            }

            return false;
        }
    }
}
