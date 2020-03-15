using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace TeleDronBot.Interfaces
{
    interface ICallbackHandler
    {
        Task BaseCallBackHandler(CallbackQueryEventArgs callback);
    }
}
