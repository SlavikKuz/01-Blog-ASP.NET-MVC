using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleDronBot.Bot
{
    class KeyboardHandler
    {
        public static IReplyMarkup ShowPartnersPilot()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Geolocation")
                    },
                    new[]
                    {
                        new KeyboardButton("Choose by region")
                    }
                },
                ResizeKeyboard = true
            };
        }
        
        public static IReplyMarkup ShowPartners()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("List of Pilots")
                    },
                },
                ResizeKeyboard = true
            };
        }

        public static IReplyMarkup EndDialog()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[] {
                new[]
                {
                    new KeyboardButton("End dialog")
                },
                },
                ResizeKeyboard = true
            };
        }

        public static IReplyMarkup PilotWithoutSubscribe_Markup()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("New Order")
                    },
                    new[]
                    {
                        new KeyboardButton("Partners")
                    },
                    new[]
                    {
                        new KeyboardButton("Orders")
                    },
                    new[]
                    {
                        new KeyboardButton("Back")
                    }
                },
                ResizeKeyboard = true
            };
        }

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

        public static IReplyMarkup Markup_Start_AfterChange()
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

        public static IReplyMarkup Markup_BuisnessmanMenu()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Orders")
                    },
                    new[]
                    {
                        new KeyboardButton("New Task")
                    },
                    new[]
                    {
                        new KeyboardButton("Back")
                    }
                },
                ResizeKeyboard = true
            };
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
                    },
                    new[]
                    {
                        new KeyboardButton("Back")
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
                    },
                    new[]
                    {
                        new KeyboardButton("Back")
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
                    },
                    new[]
                    {
                        new KeyboardButton("Back")
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
                    },
                    new[]
                    {
                        new KeyboardButton("Back")
                    }
                },
                ResizeKeyboard = true
            };
        }

        public static IReplyMarkup PilotWithSubscribe_Markup()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Orders")
                    },
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
                    },
                    new[]
                    {
                        new KeyboardButton("Back")
                    }
                },
                ResizeKeyboard = true
            };
        }

        public static IReplyMarkup AuthOrRegistration()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Authorize")
                    },
                    new[]
                    {
                        new KeyboardButton("Register")
                    }
                }
            };
        }

        public static IReplyMarkup ChangeKeyBoardPilot(int privilagie)
        {
            if (privilagie == 1)
                return PilotWithoutSubscribe_Markup();
            
            if (privilagie == 2)
                return PilotWithSubscribe_Markup();

            throw new System.Exception("incorrect value");
        }

        public static IReplyMarkup ChatConfirm()
        {
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
               {
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Confirm",CallbackData="confirm"}
                    },
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Cancel",CallbackData="cancel"}
                    }
               });
            return keyboard;
        }

        public static IReplyMarkup InviteUserToDialog()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
           {
                new[]
                {
                    new InlineKeyboardButton(){Text = "Start dialog" , CallbackData="StartDialog"}
                }
           });
        }

        public static IReplyMarkup CallBackShowOrdersForBuisnessman()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[]
                {
                    new InlineKeyboardButton(){Text = "Order" , CallbackData="RequestData"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏩", CallbackData="BuisnessNext"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏪", CallbackData="BuisnessBack"}
                }
            });
        }

        public static IReplyMarkup CallBackShowOrders()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[]
                {
                    new InlineKeyboardButton(){Text = "New task" , CallbackData="RequestTask"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏩",CallbackData="Next"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏪",CallbackData="Back"}
                }
            });
        }
    }
}
