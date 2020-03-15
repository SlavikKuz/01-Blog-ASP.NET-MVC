using System;
using System.Collections.Generic;
using System.Text;
using TeleDronBot.DTO;
using Telegram.Bot;

namespace TeleDronBot.Bot
{
    class BaseHandler
    {
        protected TelegramBotClient client;
        protected ApplicationContext db;
        public BaseHandler(TelegramBotClient client, ApplicationContext db)
        {
            this.client = client;
            this.db = db;
        }
    }
}
