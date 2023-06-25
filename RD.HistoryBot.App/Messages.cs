using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RD.HistoryBot.App
{
    internal static class Messages
    {
        public static Task AccountNotDefined(ITelegramBotClient botClient, long chatId)
            => botClient.SendTextMessageAsync(chatId,
                "К сожалению, нам не удалось Вас идентифицировать, возможно у Вас включены особые настройки приватности. Попробуйте их поменять или обратитесь к преподавателю.");

        public static Task AccountNotFound(ITelegramBotClient botClient, long chatId, string userName)
            => botClient.SendTextMessageAsync(chatId,
                $"Не найдены данные Вашего аккаунта '{userName}'. Пожалуйста, обратитесь к преподавателю, чтобы он добавил Ваш телеграм-аккаунт в таблицу пользователей этого чата.");


        public static Task AccountExpired(ITelegramBotClient botClient, long chatId, string userName)
            => botClient.SendTextMessageAsync(chatId,
                $"Истёк срок использования этого чата-бота Вашим аккаунтом {userName}. Пожалуйста, обратитесь к преподавателю, чтобы он продлил Ваше обучение.");

    }
}
