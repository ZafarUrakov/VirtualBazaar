using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Admins;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService
    {
        private async ValueTask<bool> CategoriesAsync(Update update)
        {
            var admin = this.adminService.RetrieveAllAdmins()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (admin is not null)
            {
                if(admin.AdminStatus is AdminStatus.Active
                    && update.Message.Text is categoriesCommand)
                {
                    var categories = this.categoryService.RetrieveAllCategories();
                    var markrup = CategoriesMarkup(categories);

                    string message = "Good, your categories of products 👀:";

                    if(categories is null || !categories.Any())
                    {
                        message = "Empty :(\nLet's add new category 😃";
                    }

                    await this.adminTelegramService.SendMessageAsync(
                       userTelegramId: update.Message.Chat.Id,
                       replyMarkup: markrup,
                       message: message);

                    admin.AdminStatus = AdminStatus.Categories;
                    await this.adminService.ModifyAdminAsync(admin);

                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
