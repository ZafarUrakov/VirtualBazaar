using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using VirtualBazaar.Core.Models.Foundations.Orders;
using VirtualBazaar.Core.Models.Foundations.Products;
using VirtualBazaar.Core.Models.Foundations.Users;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private async ValueTask<bool> CountOfProductAsync(Update update)
        {
            var user = userService.RetrieveAllUsers()
                .FirstOrDefault(u => u.TelegramId == update.Message.Chat.Id);

            if (user is not null)
            {
                if (await CheckIfBackCommandOnChooseCountOfProduct(update, user))
                    return true;

                if (user.UserStatus is UserStatus.Product
                    && CheckOnNumberOrNot(update.Message.Text))
                {

                    var product = this.productService
                        .RetrieveAllProducts().FirstOrDefault(p => p.Id == user.HelperId);

                    if (product.Count >= Convert.ToInt32(update.Message.Text))
                    {
                        await PopulateAndAddOrderAasync(update, product);
                        product.Count -= Convert.ToInt32(update.Message.Text);
                        await this.productService.ModifyProductAsync(product);

                        return await SendMessageAsync(update, user, product);
                    }
                    else
                    {
                        await this.userTelegramService.SendMessageAsync(
                           userTelegramId: update.Message.Chat.Id,
                           message: $"Unfortunately there is no such quantity of {product.Name}, choose another quantity :)");

                        return true;
                    }

                }
                return false;
            }
            return false;
        }

        private async Task<bool> CheckIfBackCommandOnChooseCountOfProduct(
            Update update,
            Models.Foundations.Users.User user)
        {
            if (update.Message.Text is backCommand
                && user.UserStatus is UserStatus.Product)
            {
                var product = this.productService
                    .RetrieveAllProducts().FirstOrDefault(p => p.Id == user.HelperId);

                var category = this.categoryService
                    .RetrieveAllCategories().FirstOrDefault(c => c.Id == product.CategoryId);

                var products = this.productService.RetrieveAllProducts().Where(p => p.CategoryId == category.Id);

                var markup = ProductsMarkup(products);
                user.UserStatus = UserStatus.Category;
                await this.userService.ModifyUserAsync(user);

                await this.userTelegramService.SendMessageAsync(
                       userTelegramId: update.Message.Chat.Id,
                       replyMarkup: markup,
                       message: "Let's continue 😃:");

                return true;
            }

            return false;
        }

        private async Task<bool> SendMessageAsync(
            Update update,
            Models.Foundations.Users.User user,
            Product product)
        {
            var categories = this.categoryService
                .RetrieveAllCategories();

            string message = $"Good choice, do you want to continue? 🤤";
            var markup = CategoriesMarkup(categories);

            user.UserStatus = UserStatus.Menu;
            await this.userService.ModifyUserAsync(user);

            await this.userTelegramService.SendMessageAsync(
                   userTelegramId: update.Message.Chat.Id,
                   replyMarkup: markup,
                   message: message);

            return true;
        }

        private async Task PopulateAndAddOrderAasync(Update update, Product product)
        {
            var maybeOrder = this.orderService.RetrieveAllOrders().FirstOrDefault(o => o.Name == product.Name);

            if (maybeOrder is not null)
            {
                maybeOrder.Count += Convert.ToInt32(update.Message.Text);
                await this.orderService.ModifyOrderAsync(maybeOrder);
            }
            else
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    Name = product.Name,
                    Price = product.Price,
                    Count = Convert.ToInt32(update.Message.Text)
                };

                await this.orderService.AddOrderAsync(order);
            }
        }

        private bool CheckOnNumberOrNot(string message)
        {
            Regex regex = new Regex("^[1-9]+$");
            return regex.IsMatch(message);
        }
    }
}
