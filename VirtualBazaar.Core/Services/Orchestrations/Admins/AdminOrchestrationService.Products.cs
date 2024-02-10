using System;
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
        private async ValueTask<bool> ProductsAsync(Update update)
        {
            var admin = this.adminService.RetrieveAllAdmins()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (admin is not null)
            {
                if (await CheckIfBackCommandOnChooseProducts(update, admin))
                    return true;

                var checkCategoryOrNot = CheckOnChooseCategory(update);

                if (admin.AdminStatus is AdminStatus.Categories
                    && checkCategoryOrNot is true)
                {
                    var category = this.categoryService
                        .RetrieveAllCategories().FirstOrDefault(c => c.Name == update.Message.Text);

                    string message = $"Good choice, what type of {category.Name} do you want? 🤤";

                    var products = this.productService
                        .RetrieveAllProducts().Where(p => p.CategoryId == category.Id);

                    if (products is null || !products.Any())
                    {
                        message = "Empty :(";
                    }

                    var markup = ProductsMarkup(products);
                    admin.AdminStatus = AdminStatus.Products;
                    await this.adminService.ModifyAdminAsync(admin);

                    await this.adminTelegramService.SendMessageAsync(
                           userTelegramId: update.Message.Chat.Id,
                           replyMarkup: markup,
                           message: message);

                    return true;
                }
            }

            return false;
        }

        private async Task<bool> CheckIfBackCommandOnChooseProducts(Update update, Admin admin)
        {
            if (update.Message.Text is backCommand
                && admin.AdminStatus is AdminStatus.Categories)
            {
                ReplyKeyboardMarkup markup = MenuMarkup();
                admin.AdminStatus = AdminStatus.Active;
                await this.adminService.ModifyAdminAsync(admin);

                await this.adminTelegramService.SendMessageAsync(
                           userTelegramId: update.Message.Chat.Id,
                           replyMarkup: markup,
                           message: "Let's continue 😃:");

                return true;
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
