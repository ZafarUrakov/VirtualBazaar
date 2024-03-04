using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VirtualBazaar.Core.Models.Foundations.Categories;
using VirtualBazaar.Core.Models.Foundations.Products;
using VirtualBazaar.Core.Services.Foundations.Admins;
using VirtualBazaar.Core.Services.Foundations.Categories;
using VirtualBazaar.Core.Services.Foundations.Products;
using VirtualBazaar.Core.Services.Foundations.Telegrams.Admins;
using VirtualBazaar.Core.Services.Orchestrations.Mains;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService : IAdminOrchestrationService
    {
        private readonly IMainOrchestrationService mainOrchestrationService;
        private readonly IAdminTelegramService adminTelegramService;
        private readonly IProductService productService;
        private readonly IAdminService adminService;
        private readonly ICategoryService categoryService;

        private const string startCommant = "/start";
        private const string settingsCommand = "Settings ⚙️";
        private const string categoriesCommand = "Categories 🛍";
        private const string backCommand = "⬅️ Back";

        public AdminOrchestrationService(
            IAdminTelegramService adminTelegramService,
            IProductService productService,
            IMainOrchestrationService mainOrchestrationService,
            IAdminService adminService,
            ICategoryService categoryService)
        {
            this.adminTelegramService = adminTelegramService;
            this.productService = productService;
            this.mainOrchestrationService = mainOrchestrationService;
            this.adminService = adminService;
            this.categoryService = categoryService;
        }

        public void StartWork() =>
            this.adminTelegramService.StartBot(HandleAdminMessageAsync);

        private async Task HandleAdminMessageAsync(ITelegramBotClient client, Update update, CancellationToken token)
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

            if (await CategoriesAsync(update))
                return;

            //if (await DeleteCategoryAsync(update))
            //    return;

            if (await ProductsAsync(update))
                return;
            
            if (await CheckProductAsync(update))
                return;
            
            // if (await DeleteProductAsync(update))
                // return;

            if (await BackAsync(update))
                return;

            await WrongMessageAsync(update);

            return;
        }
    }
}
