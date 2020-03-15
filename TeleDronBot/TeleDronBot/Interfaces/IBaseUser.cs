using System;
using System.Collections.Generic;
using System.Text;

namespace TeleDronBot.Interfaces
{
    interface IBaseUser : IBaseEntity
    {
        long ChatId { get; set; }
        long CurrentChatId { get; }
    }
}
