using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using TeleDronBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.Bot
{
    class AdminHandler
    {
        TelegramBotClient client;
        MainProvider provider;

        public AdminHandler(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }

        public async Task BaseAdminMessage(MessageEventArgs message)
        {

        }
    }
}
