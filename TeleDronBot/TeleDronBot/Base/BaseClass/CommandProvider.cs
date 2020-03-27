using System;
using System.Collections.Generic;
using System.Text;
using TeleDronBot.BusinessCommand;
using TeleDronBot.Chat;
using TeleDronBot.PilotCommands;
using Telegram.Bot;

namespace TeleDronBot.Base.BaseClass
{
    class CommandProvider
    {
        TelegramBotClient client;
        MainProvider provider;

        public CommandProvider(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }

        private CreateBuisnessTask _createBuisnessTask;
        private BuisnessRegistration _buisnessRegistration;
        private StopChat _stopChat;
        private RegistrationPilotCommand registrationPilotCommand;
        private ShowOrders _showOrders;
    }
}
