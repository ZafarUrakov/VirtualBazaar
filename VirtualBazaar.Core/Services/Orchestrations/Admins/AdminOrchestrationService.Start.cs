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
        private async ValueTask<bool> StartAsync(Update update)
        {
            var admin = this.adminService.RetrieveAllAdmins()
                .FirstOrDefault(a => a.TelegramId == update.Message.Chat.Id);

            if (update.Message.Text is startCommant)
            {
                if (admin is not null)
                {
                    if (admin.AdminStatus is not AdminStatus.Active)
                    {
                        admin.AdminStatus = AdminStatus.Active;
                        await this.adminService.ModifyAdminAsync(admin);
                    }

                    ReplyKeyboardMarkup menuMarkup = MenuMarkup();

                    await this.adminTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        replyMarkup: menuMarkup,
                        message: "Choose 👀:");

                    return true;
                }
                var markup = ContactMarkup();

                await this.adminTelegramService.SendMessageAsync(
                    userTelegramId: update.Message.Chat.Id,
                    replyMarkup: markup,
                    message: "Welcome to Virtual Bazaar my dear admin!\nShare ot send number of your organization please!");

                await PopulateAndAddAdminAsync(update);

                return true;
            }

            return false;
        }

        private async Task PopulateAndAddAdminAsync(Update update)
        {
            var admin = new Admin
            {
                Id = Guid.NewGuid(),
                OrganizationName = update.Message.Chat.FirstName,
                TelegramId = update.Message.Chat.Id,
                PhoneNumber = default,
                AdminStatus = AdminStatus.Register
            };

            await this.adminService.AddAdminAsync(admin);
        }
    }
}
