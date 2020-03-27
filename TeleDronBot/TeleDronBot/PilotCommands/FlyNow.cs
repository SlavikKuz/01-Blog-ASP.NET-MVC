using System;
using System.Collections.Generic;
using System.Text;
using TeleDronBot.Base.BaseClass;
using Telegram.Bot;

namespace TeleDronBot.PilotCommands
{
    class FlyNow : BaseCommand
    {
        public FlyNow(TelegramBotClient client, MainProvider provider) : base(client, provider) { }
    }
}
