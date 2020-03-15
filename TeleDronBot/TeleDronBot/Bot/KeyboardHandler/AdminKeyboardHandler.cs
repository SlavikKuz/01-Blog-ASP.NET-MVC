using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleDronBot.Bot
{
    class AdminKeyboardHandler
    {
        public static IReplyMarkup Start_Markup()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Applications")
                    },
                    new[]
                    {
                        new KeyboardButton("Moderation")
                    },
                    new[]
                    {
                        new KeyboardButton("Logs")
                    }
                },
                ResizeKeyboard = true
            };
        }
    }
}
