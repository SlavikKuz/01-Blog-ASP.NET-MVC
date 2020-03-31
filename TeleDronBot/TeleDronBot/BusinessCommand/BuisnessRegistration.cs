using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot;
using TeleDronBot.Commons;
using TeleDronBot.DTO;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.BusinessCommand
{
    class BuisnessRegistration : BaseCommand
    {
        public BuisnessRegistration(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        private async Task CommandHandler_BuisnessRegistrationKorporativ(long chatid, string message, MessageEventArgs messageObject)
        {
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);

            UserDTO user = await provider.userService.FindById(chatid);

            DronDTO dron = new DronDTO();

            if (currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Corporate registration", ++currentStep);
                await client.SendTextMessageAsync(chatid, "ВYour telephone number");
                return;
            }

            if (currentStep == 2)
            {
                if (RegularExpression.IsTelephoneCorrect(message))
                {
                    user.Phone = message;
                    user.BusinessPrivilage = 1;
                    await provider.userService.Update(user);
                    await provider.userService.ChangeAction(chatid, "NULL", 0);
                    await client.SendTextMessageAsync(chatid, "Registered", 0, false, false, 0, KeyboardHandler.Markup_BuisnessmanMenu());
                    await provider.managerPush.SendMessage(client, chatid);
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Wrong telephone number");
                }
            }
        }

    }
}
