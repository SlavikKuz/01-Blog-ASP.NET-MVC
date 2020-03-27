using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot;
using TeleDronBot.DTO;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.BusinessCommand.CallBacks
{
    class ShowMyOffersCallBack : BaseCommand
    {
        public ShowMyOffersCallBack(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task ShowOffersCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;

            ShowOrdersDTO order = await provider.showOrderService.CurrentProduct(chatid);
            int currProductId = order.CurrentProductId;

            BuisnessTaskDTO task = await provider.buisnessTaskService.FindTaskByTaskId(currProductId);

            if (task == null)
                throw new System.Exception("task cannot be null");

            List<UserDTO> offers = await provider.offerService.GetUsersOffer(currProductId);

            if (offers.Count == 0)
            {
                await client.SendTextMessageAsync(chatid, "No tasks");
                return;
            }

            offers.ForEach(async (item) =>
            {
                string message = $"ChatId user: {item.ChatId}\n " +
                $"Tel:{item.FIO}";
                await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyboardHandler.InviteUserToDialog());
            });
        }
    }
}
