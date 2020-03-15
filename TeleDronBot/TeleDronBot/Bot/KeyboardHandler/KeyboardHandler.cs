using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleDronBot.Bot
{
    class KeyboardHandler
    {

        public static IReplyMarkup Markup_Back_From_First_Action()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Back")
                    }
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup Markup_Start()
        {
            IReplyMarkup keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Buyer")
                    },
                    new[]
                    {
                        new KeyboardButton("Seller")
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }
        public static IReplyMarkup Markup_Start_BuyerMode()
        {
            IReplyMarkup keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Paid registration with insurance")
                    },
                    new[]
                    {
                        new KeyboardButton("Paid registration w/o insurance")
                    },
                    new[]
                    {
                        new KeyboardButton("Buy insurance")
                    },
                    new[]
                    {
                        new KeyboardButton("Free registration")
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }

        public static IReplyMarkup Markup_After_Registration()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Flight right away")
                    },
                    new[]
                    {
                        new KeyboardButton("Plan flight")

                    },
                    new[]
                    {
                        new KeyboardButton("Parners near")

                    },
                    new[]
                    {
                        new KeyboardButton("SOS")
                    }
                },
                ResizeKeyboard = true
            };
        }
    }
}
