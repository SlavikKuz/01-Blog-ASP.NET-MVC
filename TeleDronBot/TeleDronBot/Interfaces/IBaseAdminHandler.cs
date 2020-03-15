using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace TeleDronBot.Interfaces
{
    interface IBaseAdminHandler
    {
        Task BaseAdminMessage(MessageEventArgs message);
    }
}
