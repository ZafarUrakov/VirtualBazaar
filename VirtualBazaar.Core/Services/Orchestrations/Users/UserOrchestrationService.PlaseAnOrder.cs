using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private const string placeAnOrderCommand = "Place an order ✅";

        private async ValueTask<bool> PlaceAnOrderAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if (update.Message.Text is placeAnOrderCommand
                    && user.UserStatus is UserStatus.Basket)
                {
                    await SendMessageToAdminAsync(user);

                    user.UserStatus = UserStatus.Active;
                    await this.userService.ModifyUserAsync(user);
                    ReplyKeyboardMarkup markup = MenuMarkup();

                    await this.userTelegramService.SendMessageAsync(
                        userTelegramId: update.Message.Chat.Id,
                        replyMarkup: markup,
                        message: "Gooood, your order is accepted, please wait...🥰");

                    return true;
                }

                return false;
            }

            return false;
        }

        private async Task SendMessageToAdminAsync(Models.Foundations.Users.User user)
        {
            var orders = this.orderService.RetrieveAllOrders();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("📥 Products:\n");

            foreach (var order in orders)
            {
                stringBuilder.AppendLine($"{order.Name}");
                stringBuilder.AppendLine($"{order.Count} x {order.Price} = {order.Count * order.Price} $\n");
            }

            var totalAmount = orders.Sum(order => (double)(order.Count * order.Price));
            stringBuilder.AppendLine($"Total amount: {totalAmount} $");

            string message = $"Order ✅\n\nBuyer 👤:\nName: {user.Name}" +
                $"\nPhone number: {user.PhoneNumber}\nAddress: {user.Address}\n\n" +
                $"{stringBuilder.ToString()}";

            await this.mainOrchestrationService.SendMessageToAdminIfUserWantPlaceAnOrder(message);
        }
    }
}
