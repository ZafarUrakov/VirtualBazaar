using System.Threading.Tasks;
using System;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using System.Threading;
using Telegram.Bot;

namespace VirtualBazaar.Core.Services.Foundations.Telegrams.Users
{
    public interface IUserTelegramService
    {
        ValueTask SendMessageAsync(
            long userTelegramId,
            string message,
            int? replyToMessageId = null,
            ParseMode? parseMode = null,
            IReplyMarkup? replyMarkup = null);

        ValueTask SendPhotoAsync(
            long telegramId,
            IReplyMarkup replyMarkup,
            InputFile photo,
            string caption);

        void StartBot(
             Func<ITelegramBotClient, Update, CancellationToken, Task> handleUpdate);
    }
}
