using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot;
using TeleDronBot.DTO;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.PilotCommands
{
    class ShowOrders : BaseCommand
    {
        public ShowOrders(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task ShowAllOrders(long chatid, MessageEventArgs messageObject)
        {
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            int countTask = await provider.buisnessTaskService.CountTask();

            if (countTask == 0)
            {
                await client.SendTextMessageAsync(chatid, "No tasks");
                await provider.userService.ChangeAction(chatid, "NULL", 0);
                return;
            }
            
            BuisnessTaskDTO task = await provider.buisnessTaskService.GetFirstElement();
            string message = $"Task: {task.Id} " +
                $"Region: {task.Region} " +
                $"Description: {task.Description} " +
                $"Price: {task.Sum}";

            if (countTask == 1)
            {
                await client.SendTextMessageAsync(chatid, message);
                return;
            }

            await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyboardHandler.CallBackShowOrders());
        }
    }
    
    class CallBackOrders : ShowOrders
    {
        public CallBackOrders(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task ShowOrdersCallBack(CallbackQueryEventArgs callback)
        {
            if (callback.CallbackQuery.Data == "Next")
            {
                BuisnessTaskDTO task = await provider.s
            }
        }
    }
}
