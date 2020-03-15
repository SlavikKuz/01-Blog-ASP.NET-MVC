using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.DTO;
using TeleDronBot.Interfaces;
using TeleDronBot.Repository;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.Bot
{
    class CallBackHandler : BaseHandler, ICallbackHandler
    {
        HubRepository hubRepository;
        public CallBackHandler(TelegramBotClient client, ApplicationContext context) : base(client, context)
        {
            hubRepository = new HubRepository();
        }

        #region PrivateHandlers
        private async Task CallBackHandler_Confirm(long chatid)
        {
            HubDTO hub = await db.Hubs.Where(i => i.ChatIdReceiver == chatid).FirstOrDefaultAsync();
            await hubRepository.ConfirmDialog("Start", hub.ChatIdCreater, chatid);
        }
        #endregion

        public async Task BaseCallBackHandler(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id; // receiver
            if (callback.CallbackQuery.Data == "confirm")
            {
                await CallBackHandler_Confirm(chatid);
                HubDTO hub = await db.Hubs.Where(i => i.ChatIdReceiver == chatid).FirstOrDefaultAsync();
                long chatIdCreater = hub.ChatIdCreater;
                await client.SendTextMessageAsync(chatIdCreater, "Connected");
                await client.SendTextMessageAsync(chatid, "Connected");
                return;
            }
        }
    }
}
