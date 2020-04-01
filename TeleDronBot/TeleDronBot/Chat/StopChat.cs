using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot;
using TeleDronBot.Exceptions;
using TeleDronBot.MyException;
using Telegram.Bot;

namespace TeleDronBot.Chat
{
    class StopChat : BaseCommand
    {
        public StopChat(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task Request(long chatid)
        {
            if (!await provider.hubService.IsChatActive(chatid))
            {
                await client.SendTextMessageAsync(chatid, "No active connections");
                await ExceptionMessage.SendExceptionMessage(client, "A problem with closing dialog");
            }
            long[] chatIds = await provider.hubService.GetChatId(chatid);
            await provider.hubService.StopChat(chatid);
            await client.SendTextMessageAsync(chatid, "Select", 0, false, false, 0, KeyboardHandler.Markup_Start_AfterChange());
            await client.SendTextMessageAsync(chatIds[0], "Select", 0, false, false, 0, KeyboardHandler.Markup_Start_AfterChange());
        }
    }
}
