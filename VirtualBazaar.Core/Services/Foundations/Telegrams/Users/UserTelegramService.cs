using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Brokers.Loggings;
using VirtualBazaar.Core.Brokers.Telegrams.Admins;
using VirtualBazaar.Core.Brokers.Telegrams.Users;

namespace VirtualBazaar.Core.Services.Foundations.Telegrams.Users
{
    public class UserTelegramService : IUserTelegramService
    {
        private readonly IUserTelegramBroker userTelegramBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserTelegramService(IUserTelegramBroker userTelegramBroker,
            ILoggingBroker loggingBroker)
        {
            this.userTelegramBroker = userTelegramBroker;
            this.loggingBroker = loggingBroker;
        }

        public void StartBot(
              Func<ITelegramBotClient, Update, CancellationToken, Task> handleUpdate)
        {
            this.userTelegramBroker.StartBot(
                handleUpdate: handleUpdate);
        }

        public async ValueTask SendMessageAsync(
            long userTelegramId,
            string message,
            int? replyToMessageId = null,
            ParseMode? parseMode = null,
            IReplyMarkup? replyMarkup = null)
        {
            await this.userTelegramBroker.SendMessageAsync(
                    userTelegramId: userTelegramId,
                    message: message,
                    replyToMessageId: replyToMessageId,
                    parseMode: parseMode,
                    replyMarkup: replyMarkup);

        }

        public async ValueTask SendPhotoAsync(
            long telegramId,
            IReplyMarkup replyMarkup,
            InputFile photo, 
            string caption)
        {
            await this.userTelegramBroker.SendPhotoAsync(
                telegramId,
                replyMarkup,
                photo,
                caption);
        }
    }
}
