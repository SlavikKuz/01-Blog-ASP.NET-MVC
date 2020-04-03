using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot;
using TeleDronBot.DTO;
using TeleDronBot.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.Chat.Callback
{
    class StartDialogCallBack : BaseCallback
    {

        public StartDialogCallBack(TelegramBotClient client, MainProvider provider) : base(client, provider)
        {
        }

        public async override Task SendCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;
            string message = callback.CallbackQuery.Message.Text;

            string id = String.Empty;
            int index = message.IndexOf(":") + 1;

            for (int i = index; i < index + 10; i++)
            {
                id += message[i];
            }

            long chatIdReceiver;
            if (!long.TryParse(id, out chatIdReceiver))
                throw new System.Exception("Incorrect parse");

            UserDTO user = await provider.userService.FindById(chatid);

            BuisnessTaskDTO task = await provider.buisnessTaskService.GetCurrentTask(chatid);

            if (await provider.hubService.PilotInDialog(chatIdReceiver))
            {
                await client.SendTextMessageAsync(chatid, "Pilot joined dialog");
                return;
            }

            string messageAnswer = $"{user.FIO} wants to contact you \n " +
                $"Task in {task.Region} \n" +
                $"Description: {task.Description} ";
            await provider.hubService.CreateDialog(chatid, chatIdReceiver);
            await client.SendTextMessageAsync(chatIdReceiver, messageAnswer, 0, false, false, 0, KeyboardHandler.ChatConfirm());
        }

        public async Task StartCommenication(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;

            long[] chatIds = await provider.hubService.GetChatId(chatid);

            long chatIdReceiver = chatIds[0];

            if (chatIds.Length == 0)
                throw new System.Exception("Dialog is incorrect");

            await provider.hubService.ConfirmDialog(chatIdReceiver, chatid, true);

            await client.SendTextMessageAsync(chatIdReceiver, "Connected", 0, false, false, 0, KeyboardHandler.EndDialog());
            await client.SendTextMessageAsync(chatid, "Connected", 0, false, false, 0, KeyboardHandler.EndDialog());
        }
    }
}
