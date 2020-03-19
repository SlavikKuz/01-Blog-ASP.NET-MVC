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
                        new KeyboardButton("Pilot")
                    },
                    new[]
                    {
                        new KeyboardButton("Customer")
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }

        public static IReplyMarkup Markup_Start_Pilot_Payment_Mode()
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
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }

        public static IReplyMarkup Markup_Start_Buisness_Mode()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Private client")
                    },
                    new[]
                    {
                        new KeyboardButton("Corporate client")
                    }
                },
                ResizeKeyboard = true
            };
        }

        public static IReplyMarkup Markup_Start_Pilot_UnBuyer_Mode()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("With insurance")
                    },
                    new[]
                    {
                        new KeyboardButton("W/o insurance")
                    }
                },
                ResizeKeyboard = true
            };
        }
        
        public static IReplyMarkup Markup_Start_Pilot_Mode()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Full service (Payment)")
                    },
                    new[]
                    {
                        new KeyboardButton("Limited service (Free)")
                    }
                },
                ResizeKeyboard = true
            };
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

        public static IReplyMarkup Start_For_Corporate()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Private client")
                    },
                    new[]
                    {
                        new KeyboardButton("Corporate client")
                    }
                },
                ResizeKeyboard = true
            };
        }
    }
}
