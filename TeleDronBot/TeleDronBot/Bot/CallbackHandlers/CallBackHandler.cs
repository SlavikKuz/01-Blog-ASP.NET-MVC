using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using TeleDronBot.Interfaces;
using TeleDronBot.PilotCommands;
using TeleDronBot.Repository;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.Bot
{
    class CallBackHandler : RepositoryProvider, ICallbackHandler
    {
        TelegramBotClient client;

        MainProvider provider;
        CallBackOrders ordersCallback;

        public CallBackHandler(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
        }

        #region PrivateHandlers
        private async Task CallBackHandler_Confirm(long chatid)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);
            //await hubRepository.ConfirmDialog("Start", hub.ChatIdCreater, chatid);
        }
        #endregion

        public async Task BaseCallBackHandler(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id; // receiver
            
            if (callback.CallbackQuery.Data == "confirm")
            {
                await CallBackHandler_Confirm(chatid);
                HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);

                long chatIdCreater = hub.ChatIdCreater;
                
                await client.SendTextMessageAsync(chatIdCreater, "Connected");
                await client.SendTextMessageAsync(chatid, "Connected");
                return;
            }

            if (callback.CallbackQuery.Data == "Next" || callback.CallbackQuery.Data == "Back")
            {
                await ordersCallback.ShowOrdersCallBack(callback);
            }
        }
    }
}
