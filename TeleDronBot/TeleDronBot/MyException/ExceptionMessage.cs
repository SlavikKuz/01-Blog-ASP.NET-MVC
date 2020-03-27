using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TeleDronBot.MyException
{
    class ExceptionMessage
    {
        public async static Task SendExceptionMessage(TelegramBotClient client, string message)
        {
            await client.SendTextMessageAsync(325820574, message);
        }
    }
}
