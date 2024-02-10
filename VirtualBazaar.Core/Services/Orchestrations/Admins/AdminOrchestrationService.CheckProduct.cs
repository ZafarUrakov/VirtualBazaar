using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Admins;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService
    {
        private async ValueTask<bool> CheckProductAsync(Update update)
        {
            var admin = this.adminService.RetrieveAllAdmins()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (admin is not null)
            {
                if (await CheckIfBackCommandOnSeeProducts(update, admin))
                    return true;

                var checkProductOrNot = CheckOnChooseProduct(update);

                if (admin.AdminStatus is AdminStatus.Products
                    && checkProductOrNot is true)
                {
                    var product = this.productService
                        .RetrieveAllProducts().FirstOrDefault(p => p.Name == update.Message.Text);

                    admin.AdminStatus = AdminStatus.CheckProduct;
                    admin.HelperId = product.Id;
                    await this.adminService.ModifyAdminAsync(admin);
                    var markup = ProductMarkup();

                    await this.adminTelegramService.SendPhotoAsync(
                        telegramId: update.Message.Chat.Id,
                        replyMarkup: markup,
                        photo: InputFile.FromUri($"{product.PhotoUrl}"),
                        caption: $"Dish 🍓: {product.Name}\nPrice 💵: {product.Price}\nRemindn 💡: {product.Count}");

                    return true;
                }
                return false;
            }
            return false;
        }

        private async Task<bool> CheckIfBackCommandOnSeeProducts(Update update, Admin admin)
        {
            if (update.Message.Text is backCommand
                && admin.AdminStatus is AdminStatus.Products)
            {
                var categories = this.categoryService.RetrieveAllCategories();
                ReplyKeyboardMarkup markup = CategoriesMarkup(categories);
                admin.AdminStatus = AdminStatus.Categories;
                await this.adminService.ModifyAdminAsync(admin);

                await this.adminTelegramService.SendMessageAsync(
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
