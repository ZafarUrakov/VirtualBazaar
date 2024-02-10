using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> ProductAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if(await CheckIfBackCommandOnChooseProducts(update, user))
                    return true;

                var checkProductOrNot = CheckOnChooseProduct(update);

                if (user.UserStatus is UserStatus.Category
                    && checkProductOrNot is true)
                {
                    var product = this.productService
                        .RetrieveAllProducts().FirstOrDefault(p => p.Name == update.Message.Text);

                    user.UserStatus = UserStatus.Product;
                    user.HelperId = product.Id;
                    await this.userService.ModifyUserAsync(user);
                    var markup = ProductMarkup();

                    await this.userTelegramService.SendPhotoAsync(
                        telegramId: update.Message.Chat.Id,
                        replyMarkup: markup,
                        photo: InputFile.FromUri($"{product.PhotoUrl}"),
                        caption: $"Dish 🍓: {product.Name}\nPrice 💵: {product.Price}\nRemindn 💡: {product.Count}");

                    await this.userTelegramService.SendMessageAsync(
                    userTelegramId: update.Message.Chat.Id,
                    message: $"Choose or enter quantity of {product.Name}");

                    return true;
                }
                return false;
            }
            return false;
        }

        private async Task<bool> CheckIfBackCommandOnChooseProducts(
            Update update,
            Models.Foundations.Users.User user)
        {
            if (update.Message.Text is backCommand
                && user.UserStatus is UserStatus.Category)
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

        public bool CheckOnChooseProduct(Update update)
        {
            int count = 0;

            var products = this.productService
                       .RetrieveAllProducts();

            foreach (var product in products)
            {
                if (update.Message.Text == product.Name)
                {
                    count++;
                }
            }

            if (count > 0)
                return true;
            else
                return false;
        }
    }
}
