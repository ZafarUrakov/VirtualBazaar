using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtualBazaar.Core.Brokers.Loggings;
using VirtualBazaar.Core.Brokers.Storages;
using VirtualBazaar.Core.Brokers.Telegrams.Admins;
using VirtualBazaar.Core.Brokers.Telegrams.Users;
using VirtualBazaar.Core.Services.Foundations.Admins;
using VirtualBazaar.Core.Services.Foundations.Categories;
using VirtualBazaar.Core.Services.Foundations.Orders;
using VirtualBazaar.Core.Services.Foundations.Products;
using VirtualBazaar.Core.Services.Foundations.Telegrams.Admins;
using VirtualBazaar.Core.Services.Foundations.Telegrams.Users;
using VirtualBazaar.Core.Services.Foundations.Users;
using VirtualBazaar.Core.Services.Orchestrations.Admins;
using VirtualBazaar.Core.Services.Orchestrations.Mains;
using VirtualBazaar.Core.Services.Orchestrations.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();


builder.Services.AddScoped<IUserTelegramBroker, UserTelegramBroker>();
builder.Services.AddScoped<IAdminTelegramBroker, AdminTelegramBroker>();


builder.Services.AddTransient<IUserTelegramService, UserTelegramService>();
builder.Services.AddTransient<IAdminTelegramService, AdminTelegramService>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();

builder.Services.AddTransient<IUserOrchestrationService, UserOrchestrationService>();
builder.Services.AddTransient<IAdminOrchestrationService, AdminOrchestrationService>();
builder.Services.AddTransient<IMainOrchestrationService, MainOrchestrationService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = scopeFactory.CreateScope())
{
    var userTelegramService = scope.ServiceProvider.GetRequiredService<IUserOrchestrationService>();
    var adminTelegramService = scope.ServiceProvider.GetRequiredService<IAdminOrchestrationService>();

    userTelegramService.StartWork();
    adminTelegramService.StartWork();

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
