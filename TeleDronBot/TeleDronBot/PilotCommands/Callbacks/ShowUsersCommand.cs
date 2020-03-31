using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using Telegram.Bot;

namespace TeleDronBot.PilotCommands.Callbacks
{
    class ShowUsersCommand : BaseCommand
    {
        public ShowUsersCommand(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task Response(long chatid)
        {
            int countUser = await provider.showUserService.CountUsersAsync();
            if (countUser == 1)
            {
                await client.SendTextMessageAsync(chatid, "There are no other users");
                return;
            }
            else
            {

            }
        }
    }
}
