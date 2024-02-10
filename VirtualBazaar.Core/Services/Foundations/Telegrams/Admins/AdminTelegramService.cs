using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Brokers.Loggings;
using VirtualBazaar.Core.Brokers.Telegrams.Admins;

namespace VirtualBazaar.Core.Services.Foundations.Telegrams.Admins
{
    public class AdminTelegramService : IAdminTelegramService
    {
        private readonly IAdminTelegramBroker adminTelegramBroker;
        private readonly ILoggingBroker loggingBroker;

        public AdminTelegramService(IAdminTelegramBroker adminTelegramBroker,
            ILoggingBroker loggingBroker)
        {
            this.adminTelegramBroker = adminTelegramBroker;
            this.loggingBroker = loggingBroker;
        }

        public void StartBot(
              Func<ITelegramBotClient, Update, CancellationToken, Task> handleUpdate)
        {
            this.adminTelegramBroker.StartBot(
                handleUpdate: handleUpdate);
        }

        public async ValueTask SendPhotoAsync(
            long telegramId,
            IReplyMarkup replyMarkup,
            InputFile photo,
            string caption)
        {
            await this.adminTelegramBroker.SendPhotoAsync(
                telegramId,
                replyMarkup,
                photo,
                caption);
        }

        public async ValueTask SendMessageAsync(
            long userTelegramId,
            string message,
            int? replyToMessageId = null,
            ParseMode? parseMode = null,
            IReplyMarkup? replyMarkup = null)
        {
            await adminTelegramBroker.SendMessageAsync(
                userTelegramId: userTelegramId,
                message: message,
                parseMode: parseMode,
                replyToMessageId: replyToMessageId,
                replyMarkup: replyMarkup);
        }
    }
}
