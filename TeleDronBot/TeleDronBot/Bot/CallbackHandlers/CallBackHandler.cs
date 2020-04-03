using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.BusinessCommand.CallBacks;
using TeleDronBot.Chat.Callback;
using TeleDronBot.DTO;
using TeleDronBot.Interfaces;
using TeleDronBot.PilotCommands;
using TeleDronBot.PilotCommands.Callbacks;
using TeleDronBot.Repository;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.Bot
{
    class CallBackHandler : ICallbackHandler
    {
        TelegramBotClient client;

        MainProvider provider;
        CallBackOrders ordersCallback;
        RequestOfferCallBack offerCallback;
        ShowMyOffersCallBack myOffersCallback;
        StartDialogCallBack startDialogCallBack;

        public CallBackHandler(TelegramBotClient client, MainProvider provider)
        {
            this.provider = provider;
            this.client = client;
            ordersCallback = new CallBackOrders(client, provider);
            offerCallback = new RequestOfferCallBack(client, provider);
            myOffersCallback = new ShowMyOffersCallBack(client, provider);
            startDialogCallBack = new StartDialogCallBack(client, provider);
            CallBackShowUsers showUsersCallback;
        }

        public async Task BaseCallBackHandler(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id; // receiver

            if (callback.CallbackQuery.Data == "Next" || callback.CallbackQuery.Data == "Back")
            {
                await ordersCallback.ShowOrdersCallBack(callback);
            }

            if (callback.CallbackQuery.Data == "BusinessNext" || callback.CallbackQuery.Data == "BusinessBack")
            {
                await ordersCallback.ShowMyOrdersCallBack(callback);
            }

            if (callback.CallbackQuery.Data == "RequestTask")
            {
                await offerCallback.SendRequest(callback);
            }
            
            if (callback.CallbackQuery.Data == "RequestData")
            {
                await myOffersCallback.ShowOffersCallBack(callback);
            }

            if (callback.CallbackQuery.Data == "StartDialog")
            {
                await startDialogCallBack.SendCallBack(callback);
            }
            
            if (callback.CallbackQuery.Data == "confirm")
            {
                await startDialogCallBack.StartCommenication(callback);
            }

            if (callback.CallbackQuery.Data == "ShowUserNext" || callback.CallbackQuery.Data == "ShowUserPrevious")
            {
                await showUsersCallback.SendCallBack(callback);
            }
        }
    }
}
