using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Admins;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService
    {
        private const string changeOrganizationNameCommand = "Change organization name \U0001fa84";
        private const string changePhoneNumberCommand = "Change phone number 📲";
        private const string changeOrganizationAddressCommand = "Change organization address 🏡";

        private async ValueTask<bool> SettingsAsync(Update update)
        {
            var admin = this.adminService.RetrieveAllAdmins()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (admin is not null)
            {
                if (await HandleSettingsCommand(update, admin))
                    return true;

                if (await ChangePhoneNumberAsync(update, admin))
                    return true;

                if (await ChangeNameAsync(update, admin))
                    return true;

                if (await ChangeAddressAsync(update, admin))
                    return true;
            }

            return false;
        }

        private async ValueTask<bool> HandleSettingsCommand(Update update, Models.Foundations.Admins.Admin Admin)
        {
            if (update.Message.Text is settingsCommand)
            {
                Admin.AdminStatus = AdminStatus.Settings;
                await this.adminService.ModifyAdminAsync(Admin);
                ReplyKeyboardMarkup markup = SettingsMarkup();

                await this.adminTelegramService.SendMessageAsync(
                       userTelegramId: update.Message.Chat.Id,
                       replyMarkup: markup,
                       message: "Settings 👀:");

                return true;
            }

            return false;
        }

        private async ValueTask<bool> ChangePhoneNumberAsync(Update update, Models.Foundations.Admins.Admin Admin)
        {
            if (update.Message.Text is changePhoneNumberCommand
                && Admin.AdminStatus is AdminStatus.Settings)
            {
                Admin.AdminStatus = AdminStatus.ChangePhoneNumber;
                await this.adminService.ModifyAdminAsync(Admin);

                await this.adminTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: new ReplyKeyboardRemove(),
                      message: "Please, share or send your new phone number 👻:");

                return true;
            }
            if (Admin.AdminStatus is AdminStatus.ChangePhoneNumber)
            {
                string phoneNumber = update.Message.Text;

                if (update.Message.Type is MessageType.Contact)
                {
                    phoneNumber = update.Message.Contact.PhoneNumber;
                }

                if (CheckPhoneNumber(phoneNumber))
                {
                    Admin.PhoneNumber = phoneNumber;
                    Admin.AdminStatus = AdminStatus.Active;
                    await this.adminService.ModifyAdminAsync(Admin);
                    ReplyKeyboardMarkup markup = MenuMarkup();

                    await this.adminTelegramService.SendMessageAsync(
                          userTelegramId: update.Message.Chat.Id,
                          replyMarkup: markup,
                          message: "Changed 😁");
                }
                else
                {
                    await this.adminTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        message: "Number is invalid, please try again.");

                }

                return true;
            }

            return false;
        }

        private async ValueTask<bool> ChangeNameAsync(Update update, Models.Foundations.Admins.Admin Admin)
        {
            if (update.Message.Text is changeOrganizationNameCommand
                && Admin.AdminStatus is AdminStatus.Settings)
            {
                Admin.AdminStatus = AdminStatus.ChangeName;
                await this.adminService.ModifyAdminAsync(Admin);

                await this.adminTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: new ReplyKeyboardRemove(),
                      message: "Please, send your new organization name 👻:");

                return true;
            }
            if (Admin.AdminStatus is AdminStatus.ChangeName)
            {
                Admin.OrganizationName = update.Message.Text;
                Admin.AdminStatus = AdminStatus.Active;
                await this.adminService.ModifyAdminAsync(Admin);
                ReplyKeyboardMarkup markup = MenuMarkup();

                await this.adminTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: markup,
                      message: "Changed 😁");

                return true;
            }

            return false;
        }

        private async ValueTask<bool> ChangeAddressAsync(Update update, Models.Foundations.Admins.Admin Admin)
        {
            if (update.Message.Text is changeOrganizationAddressCommand
                && Admin.AdminStatus is AdminStatus.Settings)
            {
                Admin.AdminStatus = AdminStatus.ChangeAddress;
                await this.adminService.ModifyAdminAsync(Admin);

                await this.adminTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: new ReplyKeyboardRemove(),
                      message: "Please, share or send your new organization address 🏡:");

                return true;
            }
            if (Admin.AdminStatus is AdminStatus.ChangeAddress)
            {
                string address = update.Message.Text;
                address = await IfTypeOfMessageIsLocation(update, address);

                Admin.Address = address;
                Admin.AdminStatus = AdminStatus.Active;
                await this.adminService.ModifyAdminAsync(Admin);
                ReplyKeyboardMarkup markup = MenuMarkup();

                await this.adminTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: markup,
                      message: "Changed 😁");

                return true;
            }

            return false;
        }
    }
}
