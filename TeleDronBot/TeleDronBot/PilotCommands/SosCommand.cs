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
    class SosCommand : BaseCommand
    {
        public SosCommand(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task SosHandler(MessageEventArgs messageObject)
        {
            long chatid = messageObject.Message.Chat.Id;
            string message = messageObject.Message.Text;

            int currStep = await provider.userService.GetCurrentActionStep(chatid);
            SosDTO sos = await provider.sosTableServide.FindById(chatid);

            if (currStep == 0)
            {
                await provider.userService.ChangeAction(chatid, "SOS", ++currStep);
                return;
            }

            if (currStep == 1)
            {
                if (message == "Insurance" || message == "Accident")
                {
                    if (message == "Insurance")
                    {
                        await client.SendTextMessageAsync(chatid, "Error data");
                        return;
                    }
                    else if (message == "Accident")
                    {
                        await provider.sosTableServide.Create(new SosDTO
                        {
                            ChatId = chatid,
                            Type = 1
                        });
                        await client.SendTextMessageAsync(chatid, "your geolocation", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                        await provider.userService.ChangeAction(chatid, "SOS", ++currStep);
                        return;
                    }
                    else
                    {
                        await client.SendTextMessageAsync(chatid, "Wrong option");
                    }
                }

                if (currStep == 2)
                {
                    if (sos.Type == 1)
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
}
