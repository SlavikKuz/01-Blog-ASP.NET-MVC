using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.BusinessCommand
{
    class CreateBuisnessTask
    {
        TelegramBotClient client;
        MainProvider provider;
        public CreateBuisnessTask(MainProvider provider, TelegramBotClient client)
        {
            this.provider = provider;
            this.client = client;
        }
        public async Task CreateTask(long chatid, string message, MessageEventArgs messageObject)
        {
            bool isUserBuisnessman = await provider.buisnessTaskService.IsUserBuisnessman(chatid);
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            BuisnessTaskDTO currTask = await provider.buisnessTaskService.FindTask(chatid);
            
            if (!isUserBuisnessman)
            {
                await client.SendTextMessageAsync(chatid, "Please, register as client");
                return;
            }
            
            if (currentStep == 1)
            {
                BuisnessTaskDTO newTask = new BuisnessTaskDTO()
                {
                    ChatId = chatid,
                    Region = message
                };
                await provider.buisnessTaskService.Create(newTask);
                await client.SendTextMessageAsync(chatid, "Enter your task/order");
                await provider.userService.ChangeAction(chatid, "Create task", ++currentStep);
                return;
            }
            
            if (currentStep == 2)
            {
                currTask.Description = message;
                await provider.buisnessTaskService.Update(currTask);
                await provider.userService.ChangeAction(chatid, "Create task", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Your budget");
                return;
            }

            if (currentStep == 3)
            {
                currTask.Sum = Convert.ToInt32(message);
                await provider.buisnessTaskService.Update(currTask);
                await client.SendTextMessageAsync(chatid, "Task created");
                return;
            }
        }
    }
}
