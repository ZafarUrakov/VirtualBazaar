using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> RegisterAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if (user.UserStatus is UserStatus.Register)
                {
                    string phoneNumber = update.Message.Text;

                    if (update.Message.Type is MessageType.Contact)
                    {
                        phoneNumber = update.Message.Contact.PhoneNumber;
                    }
                    if (CheckUserPhoneNumber(phoneNumber))
                    {
                        user.PhoneNumber = phoneNumber;
                        user.UserStatus = UserStatus.Contact;
                        await this.userService.ModifyUserAsync(user);

                        ReplyKeyboardMarkup markup = LocationMarkup();

                        await this.userTelegramService.SendMessageAsync(
                            userTelegramId: update.Message.Chat.Id,
                            replyMarkup: markup,
                            message: "Good, please share or send your address 📍:");
                    }
                    else
                    {
                        await this.userTelegramService.SendMessageAsync(
                            userTelegramId: update.Message.Chat.Id,
                            message: "Number is invalid, please try again.");
                    }

                    return true;
                }
                if (user.UserStatus is UserStatus.Contact)
                {
                    string address = update.Message.Text;
                    address = await IfTypeOfMessageIsLocation(update, address);
                    user.Address = address;
                    user.UserStatus = UserStatus.Active;
                    await this.userService.ModifyUserAsync(user);
                    ReplyKeyboardMarkup markup = MenuMarkup();

                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        replyMarkup: markup,
                        message: "Good, choose the options please 👀:");

                    return true;
                }
            }

            return false;
        }

        private bool CheckUserPhoneNumber(string phoneNumber)
        {
            if (phoneNumber[0] == '+'
                && phoneNumber.Length >= 8)
            {
                return true;
            }

            return false;
        }
    }
}
