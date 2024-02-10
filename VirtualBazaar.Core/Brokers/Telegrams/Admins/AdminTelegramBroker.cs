using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace VirtualBazaar.Core.Brokers.Telegrams.Admins
{
    public class AdminTelegramBroker : IAdminTelegramBroker
    {
        private readonly ITelegramBotClient telegramBotClient;

        public AdminTelegramBroker()
        {
            string token = "6486297329:AAFZeruXJHWP1FPh5hVtRAjIw_xCC0AByJk";
            this.telegramBotClient = new TelegramBotClient(token);
        }

        public void StartBot(
             Func<ITelegramBotClient, Update, CancellationToken, Task> handleUpdate)
        {
            this.telegramBotClient.StartReceiving(
                updateHandler: handleUpdate,
                pollingErrorHandler: HandleErrorAsync);
        }

        public async ValueTask SendMessageAsync(
           long userTelegramId,
           string message,
           int? replyToMessageId = null,
           ParseMode? parseMode = null,
           IReplyMarkup? replyMarkup = null)
        {
            await this.telegramBotClient.SendTextMessageAsync(
                    chatId: userTelegramId,
                    text: message,
                    replyToMessageId: replyToMessageId,
                    parseMode: parseMode,
                    replyMarkup: replyMarkup);

        }

        public async ValueTask SendPhotoAsync(
            long telegramId,
            IReplyMarkup replyMarkup,
            InputFile photo,
            string caption) =>
            await this.telegramBotClient.SendPhotoAsync(
               chatId: telegramId,
               replyMarkup: replyMarkup,
               photo: photo,
               caption: caption);

        private async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            await client.SendTextMessageAsync(
               chatId: 1924521160,
               text: $"Error: {exception}");
        }
    }
}
