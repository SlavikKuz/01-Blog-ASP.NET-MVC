using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot;
using TeleDronBot.DTO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleDronBot.PilotCommands.Callbacks
{
    class CallBackOrders : ShowOrders
    {
        public CallBackOrders(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task ShowMyOrdersCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;
            
            if (callback.CallbackQuery.Data == "BuisnessNext")
            {
                BuisnessTaskDTO task = await provider.showOrderService.GetNextProduct(chatid, true);
            
                if (task == null)
                {
                    await client.SendTextMessageAsync(chatid, "Last task");
                    return;
                }
                
                int messageId = await provider.showOrderService.GetMessageId(chatid);
                
                string message = $"Order: {task.Id} \n" +
                   $"Region: {task.Region} \n" +
                   $"Description: {task.Description} \n" +
                   $"Sum: {task.Sum}";
                await client.EditMessageTextAsync(chatid, messageId + 1, message, 0, false, (InlineKeyboardMarkup)KeyboardHandler.CallBackShowOrdersForBuisnessman());
            }
            
            if (callback.CallbackQuery.Data == "BuisnessBack")
            {
                BuisnessTaskDTO task = await provider.showOrderService.GetPreviousProduct(chatid, true);
                
                if (task == null)
                {
                    await client.SendTextMessageAsync(chatid, "First task");
                }
                
                int messageId = await provider.showOrderService.GetMessageId(chatid);
                
                string message = $"Order: {task.Id} \n" +
                   $"Region: {task.Region} \n" +
                   $"Description: {task.Description} \n" +
                   $"Sum: {task.Sum}";
                
                await client.EditMessageTextAsync(chatid, messageId + 1, message, 0, false, (InlineKeyboardMarkup)KeyboardHandler.CallBackShowOrdersForBuisnessman());
            }
        }

        public async Task ShowOrdersCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;

            if (callback.CallbackQuery.Data == "RequestTask")
            {
                UserDTO user = await provider.userService.FindById(chatid);
                if (user == null)
                    throw new System.Exception("User cannot be null");
            }

            if (callback.CallbackQuery.Data == "Next")
            {
                BuisnessTaskDTO task = await provider.showOrderService.GetNextProduct(chatid);
            
                if (task == null)
                {
                    await client.SendTextMessageAsync(chatid, "Last task");
                    return;
                }
                
                int messageId = await provider.showOrderService.GetMessageId(chatid);
                
                string message = $"Order: {task.Id} \n" +
                   $"Region: {task.Region} \n" +
                   $"Description: {task.Description} \n" +
                   $"Sum: {task.Sum}";
                
                await client.EditMessageTextAsync(chatid, messageId + 1, message, 0, false, (InlineKeyboardMarkup)KeyboardHandler.CallBackShowOrders());
            }
            
            if (callback.CallbackQuery.Data == "Back")
            {
                BuisnessTaskDTO task = await provider.showOrderService.GetPreviousProduct(chatid);
                
                if (task == null)
                {
                    await client.SendTextMessageAsync(chatid, "First task");
                    return;
                }
                
                int messageId = await provider.showOrderService.GetMessageId(chatid);
                
                string message = $"Order: {task.Id} \n" +
                   $"Region: {task.Region} \n" +
                   $"Description: {task.Description} \n" +
                   $"Sum: {task.Sum}";
                
                await client.EditMessageTextAsync(chatid, messageId + 1, message, 0, false, (InlineKeyboardMarkup)KeyboardHandler.CallBackShowOrders());
            }
        }
    }
}
