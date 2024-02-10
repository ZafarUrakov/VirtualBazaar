using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> BasketAsync(Update update)
        {
            var user = this.userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if (update.Message.Text is basketCommand)
                {
                    var orders = this.orderService.RetrieveAllOrders();
                    var markup = BasketMarkup(orders);
                    string message = CreateMessageForbasket();

                    if (message == "Bakset is empty :(")
                    {
                        await this.userTelegramService.SendMessageAsync(
                              userTelegramId: update.Message.Chat.Id,
                              message: message);

                        return true;
                    }

                    user.UserStatus = UserStatus.Basket;
                    await this.userService.ModifyUserAsync(user);

                    await this.userTelegramService.SendMessageAsync(
                       userTelegramId: update.Message.Chat.Id,
                       replyMarkup: markup,
                       message: message);

                    return true;
                }

                return false;
            }

            return false;
        }

        private string CreateMessageForbasket()
        {
            var message = "";
            var orders = this.orderService.RetrieveAllOrders();

            if (orders is null || !orders.Any())
                return message = "Bakset is empty :(";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("📥 Basket:\n");

            foreach (var order in orders)
            {
                stringBuilder.AppendLine($"{order.Name}");
                stringBuilder.AppendLine($"{order.Count} x {order.Price} = {order.Count * order.Price} $\n");
            }

            var totalAmount = orders.Sum(order => (double)(order.Count * order.Price));
            stringBuilder.AppendLine($"Total amount: {totalAmount} $");

            message = stringBuilder.ToString();
            return message;
        }
    }
}
