using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.PilotCommands
{
    class SosCommand : BaseCommand
    {
        public SosCommand(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task SosHandler(MessageEventArgs messageObject)
        {
            long chatid = messageObject.Message.Chat.Id;
            string message = messageObject.Message.Text;

            int currStep = await provider.userService.GetCurrentActionStep(chatid);
            SosDTO sos = await provider.sosTableServide.FindById(chatid);

            if (currStep == 1)
            {
                if (message == "Insurance")
                {
                    await client.SendTextMessageAsync(chatid, "Data Error");
                    return;
                }
                else if (message == "Accident")
                {
                    await provider.sosTableServide.Create(new SosDTO
                    {
                        ChatId = chatid,
                        Type = true
                    });
                    await client.SendTextMessageAsync(chatid, "Your geolocation");
                    await provider.userService.ChangeAction(chatid, "SOS", ++currStep);
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Please select");
                    return;
                }
            }
            if (currStep == 2)
            {
                if (sos.Type == true)
                {
                    if (messageObject.Message.Location != null)
                    {
                        sos.lautitude = messageObject.Message.Location.Latitude;
                        sos.longtitude = messageObject.Message.Location.Longitude;
                        await provider.sosTableServide.Update(sos);
                        List<long> lstId = await provider.userService.GetUsersIdByRegion(chatid);

                        UserDTO user = await provider.userService.FindById(chatid);

                        string _message = $"Pilot {user.ChatId} has problem \n" +
                            $"Tel. {user.Phone} \n " +
                            $"Description";

                        foreach (var i in lstId)
                        {
                            await client.SendTextMessageAsync(i, _message);
                            await client.SendLocationAsync(chatid, sos.lautitude, sos.longtitude);
                        }
                        await provider.userService.ChangeAction(chatid, "NULL", 0);
                        await client.SendTextMessageAsync(chatid, "The problem reported.");
                        return;
                    }

                }
            }
        }
    }
}
