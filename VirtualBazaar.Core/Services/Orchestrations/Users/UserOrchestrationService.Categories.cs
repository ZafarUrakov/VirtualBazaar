using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Telegram.Bot.Types;
using VirtualBazaar.Core.Models.Foundations.Categories;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> CategoriesAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                var checkCategoryOrNot = CheckOnChooseCategory(update);

                if (user.UserStatus is UserStatus.Menu 
                    && checkCategoryOrNot is true)
                {

                    var category = this.categoryService
                        .RetrieveAllCategories().FirstOrDefault(c => c.Name == update.Message.Text);

                    string message = $"Good choice, what type of {category.Name} do you want? 🤤";

                    var products = this.productService
                        .RetrieveAllProducts().Where(p => p.CategoryId == category.Id);

                    if(products is null || !products.Any())
                    {
                        message = "Empty :(";
                    }

                    var markup = ProductsMarkup(products);
                    user.UserStatus = UserStatus.Category;
                    await this.userService.ModifyUserAsync(user);

                    await this.userTelegramService.SendMessageAsync(
                           userTelegramId: update.Message.Chat.Id,
                           replyMarkup: markup,
                           message: message);

                    return true;
                }
            }

            return false;
        }

        public bool CheckOnChooseCategory(Update update)
        {
            int count = 0;

            var categories = this.categoryService
                       .RetrieveAllCategories();

            foreach (var category in categories)
            {
                if (update.Message.Text == category.Name)
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
