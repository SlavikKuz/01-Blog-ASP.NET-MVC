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
    class CallBackShowUsers : BaseCallback
    {
        public CallBackShowUsers(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async override Task SendCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;
            string message;
            if (callback.CallbackQuery.Data == "ShowUserNext")
            {
                UserDTO user = await provider.showUserService.GetNextUser(chatid);
                if (user == null)
                {
                    await client.SendTextMessageAsync(chatid, "Last user");
                    return;
                }
                
                int messageId = await provider.showUserService.GetMessageId(chatid);
                message = $"Pilot:{user.FIO} \n" +
                            $"Tel:{user.Phone}";
                await client.EditMessageTextAsync(chatid, messageId + 2, message, 0, false, (InlineKeyboardMarkup)KeyboardHandler.CallBackShowForUser());
                return;
            }
            
            if (callback.CallbackQuery.Data == "ShowUserPrevious")
            {
                UserDTO user = await provider.showUserService.GetPreviousUser(chatid);
                if (user == null)
                {
                    await client.SendTextMessageAsync(chatid, "First user");
                    return;
                }
                
                int messageId = await provider.showUserService.GetMessageId(chatid);
                message = $"Pilot:{user.FIO} \n" +
                          $"Tel:{user.Phone}";
                await client.EditMessageTextAsync(chatid, messageId + 2, message, 0, false, (InlineKeyboardMarkup)KeyboardHandler.CallBackShowForUser());
                return;
            }
        }
    }
}
