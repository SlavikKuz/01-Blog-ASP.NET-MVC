using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.PilotCommands.Callbacks
{
    class RequestOfferCallBack : BaseCommand
    {
        public RequestOfferCallBack(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task SendRequest(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;

            ShowOrdersDTO order = await provider.showOrderService.CurrentProduct(chatid);

            List<int> idProducts = await provider.showOrderService.GetIdTasksForUser(chatid);
            if (idProducts.Contains(order.CurrentProductId))
            {
                await client.SendTextMessageAsync(chatid, "It's your order");
                return;
            }

            BuisnessTaskDTO task = await provider.buisnessTaskService.FindTaskByTaskId(order.CurrentProductId);


            OfferDTO offer = new OfferDTO
            {
                ChatId = chatid,
                TaskId = task.Id
            };

            await provider.offerService.Create(chatid, offer);
            await client.SendTextMessageAsync(chatid, "Created");
        }
    }
}
