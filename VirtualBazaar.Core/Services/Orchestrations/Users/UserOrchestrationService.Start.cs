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
        private async ValueTask<bool> StartAsync(Update update)
        {
            if (update.Message.Text is startCommant)
            {
                var user = this.userService.RetrieveAllUsers()
                    .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

                if (user is not null)
                {
                    if (user.UserStatus is not UserStatus.Active)
                    {
                        user.UserStatus = UserStatus.Active;
                        await this.userService.ModifyUserAsync(user);
                    }

                    ReplyKeyboardMarkup menuMarkup = MenuMarkup();

                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        replyMarkup: menuMarkup,
                        message: "Choose 👀:");

                    return true;
                }

                ReplyKeyboardMarkup markup = ContactMarkup();

                await this.userTelegramService.SendMessageAsync(
                    userTelegramId: update.Message.Chat.Id,
                    replyMarkup: markup,
                    message: "Welcome to Virtual Bazaar!\nShare or send your number please!");

                await PopulateAndAddUserAsync(update);

                return true;
            }

            return false;
        }

        private async Task PopulateAndAddUserAsync(Update update)
        {
            var user = new Models.Foundations.Users.User
            {
                Id = Guid.NewGuid(),
                Name = update.Message.Chat.FirstName,
                TelegramId = update.Message.Chat.Id,
                PhoneNumber = default,
                UserStatus = UserStatus.Register
            };

            await this.userService.AddUserAsync(user);
        }
    }
}
