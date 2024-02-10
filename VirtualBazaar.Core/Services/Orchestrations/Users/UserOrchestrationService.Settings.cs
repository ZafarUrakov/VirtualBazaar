using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private const string changeNameCommand = "Change name 🪄";
        private const string changePhoneNumberCommand = "Change phone number 📲";
        private const string changeAddressCommand = "Change address 🏡";

        private async ValueTask<bool> SettingsAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if(await HandleSettingsCommand(update, user))
                    return true;

                if(await ChangePhoneNumberAsync(update, user))
                    return true;

                if (await ChangeNameAsync(update, user))
                    return true;

                if(await ChangeAddressAsync(update, user))
                    return true;
            }

            return false;
        }

        private async ValueTask<bool> HandleSettingsCommand(Update update, Models.Foundations.Users.User user)
        {
            if (update.Message.Text is settingsCommand)
            {
                user.UserStatus = UserStatus.Settings;
                await this.userService.ModifyUserAsync(user);
                ReplyKeyboardMarkup markup = SettingsMarkup();

                await this.userTelegramService.SendMessageAsync(
                       userTelegramId: update.Message.Chat.Id,
                       replyMarkup: markup,
                       message: "Settings 👀:");

                return true;
            }

            return false;
        }

        private async ValueTask<bool> ChangePhoneNumberAsync(Update update, Models.Foundations.Users.User user)
        {
            if (update.Message.Text is changePhoneNumberCommand
                && user.UserStatus is UserStatus.Settings)
            {
                user.UserStatus = UserStatus.ChangePhoneNumber;
                await this.userService.ModifyUserAsync(user);

                await this.userTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: new ReplyKeyboardRemove(),
                      message: "Please, share or send your new phone number 👻:");

                return true;
            }
            if (user.UserStatus is UserStatus.ChangePhoneNumber)
            {
                string phoneNumber = update.Message.Text;

                if (update.Message.Type is MessageType.Contact)
                {
                    phoneNumber = update.Message.Contact.PhoneNumber;
                }

                if (CheckUserPhoneNumber(phoneNumber))
                {
                    user.PhoneNumber = phoneNumber;
                    user.UserStatus = UserStatus.Active;
                    await this.userService.ModifyUserAsync(user);
                    ReplyKeyboardMarkup markup = MenuMarkup();

                    await this.userTelegramService.SendMessageAsync(
                          userTelegramId: update.Message.Chat.Id,
                          replyMarkup: markup,
                          message: "Changed 😁");
                }
                else
                {
                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        message: "Number is invalid, please try again.");

                }

                return true;
            }

            return false;
        }

        private async ValueTask<bool> ChangeNameAsync(Update update, Models.Foundations.Users.User user)
        {
            if (update.Message.Text is changeNameCommand
                && user.UserStatus is UserStatus.Settings)
            {
                user.UserStatus = UserStatus.ChangeName;
                await this.userService.ModifyUserAsync(user);

                await this.userTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: new ReplyKeyboardRemove(),
                      message: "Please, send your new name 👻:");

                return true;
            }
            if (user.UserStatus is UserStatus.ChangeName)
            {
                user.Name = update.Message.Text;
                user.UserStatus = UserStatus.Active;
                await this.userService.ModifyUserAsync(user);
                ReplyKeyboardMarkup markup = MenuMarkup();

                await this.userTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: markup,
                      message: "Changed 😁");

                return true;
            }

            return false;
        }
        
        private async ValueTask<bool> ChangeAddressAsync(Update update, Models.Foundations.Users.User user)
        {
            if (update.Message.Text is changeAddressCommand
                && user.UserStatus is UserStatus.Settings)
            {
                user.UserStatus = UserStatus.ChangeAddress;
                await this.userService.ModifyUserAsync(user);

                await this.userTelegramService.SendMessageAsync(
                      userTelegramId: update.Message.Chat.Id,
                      replyMarkup: new ReplyKeyboardRemove(),
                      message: "Please, share or send your new address 🏡:");

                return true;
            }
            if (user.UserStatus is UserStatus.ChangeAddress)
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
                      message: "Changed 😁");

                return true;
            }

            return false;
        }

        private static async Task<string> IfTypeOfMessageIsLocation(Update update, string address)
        {
            if (update.Message.Type is MessageType.Location)
            {
                double longitude = update.Message.Location.Longitude;
                double latitude = update.Message.Location.Latitude;


                using (var httpClient = new HttpClient())
                {
                    string apiKey = "e2e8a7f702ae48b0b602f87993c98955";
                    string
                        apiUrlForLocation = $"https://api.opencagedata" +
                        $".com/geocode/v1/json?key={apiKey}&q={latitude}+{longitude}";

                    HttpResponseMessage responseForLocation = await httpClient.GetAsync(apiUrlForLocation);
                    string contentForLocation = await responseForLocation.Content.ReadAsStringAsync();
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(contentForLocation);

                    string city = result.results[0].components.city;
                    string street = result.results[0].components.road;
                    string houseNumber = result.results[0].components.house_number;

                    address = $"{city}, {street}, {houseNumber}";
                }
            }

            return address;
        }
    }
}
