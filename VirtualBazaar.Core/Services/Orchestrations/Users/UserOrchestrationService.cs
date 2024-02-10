using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using VirtualBazaar.Core.Models.Foundations.Categories;
using VirtualBazaar.Core.Models.Foundations.Products;
using VirtualBazaar.Core.Services.Foundations.Admins;
using VirtualBazaar.Core.Services.Foundations.Categories;
using VirtualBazaar.Core.Services.Foundations.Orders;
using VirtualBazaar.Core.Services.Foundations.Products;
using VirtualBazaar.Core.Services.Foundations.Telegrams.Users;
using VirtualBazaar.Core.Services.Foundations.Users;
using VirtualBazaar.Core.Services.Orchestrations.Mains;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService : IUserOrchestrationService
    {
        private readonly IMainOrchestrationService mainOrchestrationService;
        private readonly IUserTelegramService userTelegramService;
        private readonly IUserService userService;
        private readonly IOrderService orderService;
        private readonly IAdminService adminService;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private const string startCommant = "/start";
        private const string settingsCommand = "Settings ⚙️";
        private const string backCommand = "⬅️ Back";
        private const string basketCommand = "📥 Basket";
        private const string clearBasketCommand = "🔄 Clear";

        public UserOrchestrationService(
            IUserTelegramService userTelegramService,
            IOrderService orderService,
            IMainOrchestrationService mainOrchestrationService,
            IUserService userService,
            IAdminService adminService,
            ICategoryService categoryService,
            IProductService productService)
        {
            this.userTelegramService = userTelegramService;
            this.orderService = orderService;
            this.mainOrchestrationService = mainOrchestrationService;
            this.userService = userService;
            this.adminService = adminService;
            this.categoryService = categoryService;
            this.productService = productService;
        }

        public void StartWork() =>
            this.userTelegramService.StartBot(HandleUserMessageAsync);

        private async Task HandleUserMessageAsync(
            ITelegramBotClient client,
            Update update,
            CancellationToken token)
        {

            //var category = new Category
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Shashlik"
            //};
            //await this.categoryService.AddCategoryAsync(category);

            //var product = new Product
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Shashlik gushli",
            //    Price = 1000,
            //    Count = 50,
            //    PhotoUrl = "https://static.insales-cdn.com/images/products/1/2641/245615185/%D1%81%D0%B0%D0%BC%D1%81%D0%B0_%D1%81_%D0%B3%D0%BE%D0%B2%D1%8F%D0%B4.jpg",
            //    CategoryId = category.Id
            //}; ;
            //await this.productService.AddProductAsync(product);


            if (await StartAsync(update))
                return;

            if (await RegisterAsync(update))
                return;

            if (await SettingsAsync(update))
                return;

            if (await MeAsync(update))
                return;

            if (await ContactUsAsync(update))
                return;

            if (await MenuAsync(update))
                return;

            if (await CategoriesAsync(update))
                return;

            if (await ProductAsync(update))
                return;

            if (await CountOfProductAsync(update))
                return;

            if (await BasketAsync(update))
                return;

            if (await ClearBasketAsync(update))
                return;

            if (await DeleteOrderAsync(update))
                return;

            if (await PlaceAnOrderAsync(update))
                return;

            if (await BackAsync(update))
                return;

            await WrongMessageAsync(update);

            return;
        }
    }
}
