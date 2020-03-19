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
    class AdminHandler : RepositoryProvider
    {
        public AdminHandler(TelegramBotClient client) { }

        public async Task BaseAdminMessage(MessageEventArgs message)
        {

        }
    }
}
