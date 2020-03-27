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

        public async Task ShowAllOrders(long chatid, MessageEventArgs messageObject, bool isBusinessman = false)
        {
            int countTask;
            BuisnessTaskDTO task;
            string message;
            
            // for business 
            if (isBusinessman)
            {
                countTask = await provider.buisnessTaskService.CountTask(chatid);
                
                if (countTask == 0)
                {
                    await client.SendTextMessageAsync(chatid, "No tasks");
                }

                task = await provider.buisnessTaskService.GetFirstElement(chatid);

                message = $"Order: {task.Id} \n" +
                   $"Region: {task.Region} \n" +
                   $"Description: {task.Description} \n" +
                   $"Sum: {task.Sum}";
                
                await provider.showOrderService.SetDefaultProduct(chatid, true);
                await provider.showOrderService.ChangeMessageId(chatid, messageObject.Message.MessageId);
                await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyboardHandler.CallBackShowOrdersForBuisnessman());

                return;
            }
            countTask = await provider.buisnessTaskService.CountTask();

            if (countTask == 0)
            {
                await client.SendTextMessageAsync(chatid, "No tasks");
                await provider.userService.ChangeAction(chatid, "NULL", 0);
                return;
            }
            
            task = await provider.buisnessTaskService.GetFirstElement();
            message = $"Task: {task.Id}  \n" +
                $"Region: {task.Region}  \n" +
                $"Description: {task.Description}  \n" +
                $"Price: {task.Sum}";

            await provider.showOrderService.SetDefaultProduct(chatid);
            await provider.showOrderService.ChangeMessageId(chatid, messageObject.Message.MessageId);

            await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyboardHandler.CallBackShowOrders());
        }
    }
   
}
