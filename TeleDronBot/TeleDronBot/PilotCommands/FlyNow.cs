using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.PilotCommands
{
    class FlyNow : BaseCommand
    {
        public FlyNow(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task Fly(MessageEventArgs messageObject)
        {
            string message = messageObject.Message.Text;
            long chatid = messageObject.Message.Chat.Id;

            int currStep = await provider.userService.GetCurrentActionStep(chatid);

            if (currStep == 1)
            {
                if (messageObject.Message.Location == null)
                {
                    await client.SendTextMessageAsync(chatid, "Geolocation failed. Try again");
                    return;
                }
                float longtitude = messageObject.Message.Location.Longitude;
                float lautitude = messageObject.Message.Location.Latitude;


            }
        }
    }
}
