using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot.CommonHandler;
using TeleDronBot.DTO;
using TeleDronBot.Geolocation;
using TeleDronBot.Interfaces.Bot;
using TeleDronBot.Logs;
using TeleDronBot.Repository;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeleDronBot.Bot
{
    class MessageHandler : MainProvider, IMessageHandler
    {
        long chatid;
        TelegramBotClient client;

        public MessageHandler(TelegramBotClient client)
        {
            this.client = client;
        }

        #region PrivateHandlers

        #region BuisnessRegistration
        private async Task CommandHandler_BuisnessRegistrationKorporativ(long chatid, string message, MessageEventArgs messageObject)
        {
            int currentStep = await userRepository.GetCurrentActionStep(chatid);
            UserDTO user = await userRepository.FindById(chatid);
            DronDTO dron = new DronDTO();

            if (currentStep == 1)
            {
                user.FIO = message;
                await userRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Corporate client registration", 2);
                await client.SendTextMessageAsync(chatid, "Enter your telephone number");
                return;
            }

            if (currentStep == 2)
            {
                user.Phone = message;
                await userRepository.Update(user);
                await client.SendTextMessageAsync(chatid, "Registation succeeded");
                await managerPush.SendMessage(client, chatid);
                return;
            }
        }
        #endregion

        private async Task CommandHandler_Start(long chatid)
        {
            await client.SendTextMessageAsync(chatid, "Sample text", 0, false, false, 0, KeyboardHandler.Markup_Start());
        }

        #region PaymantRegistration

        private async Task CommandHandler_PaidRegistrationWithoutInsurance(long chatid, string message, MessageEventArgs messageObject)
        {
            int currentStep = await userRepository.GetCurrentActionStep(chatid);

            UserDTO user = await userRepository.FindById(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await proposalRepository.FindById(chatid);

            if (currentStep == 1)
            {
                user.FIO = message;
                await userRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Paid registarion w/o insurance", 2);
                await client.SendTextMessageAsync(chatid, "Enter mobile phone number");
                return;
            }
            if (currentStep == 2)
            {
                user.Phone = message;
                await userRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Paid registarion w/o insurance", 3);
                await client.SendTextMessageAsync(chatid, "Enter drone type");
                return;
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.Create(dron);
                await userRepository.ChangeAction(chatid, "Paid registarion w/o insurance", 4);
                await client.SendTextMessageAsync(chatid, "Enter your address");
            }
            if (currentStep == 4)
            {
                await proposalRepository.Create(user);
                proposal.Address = message;
                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Paid registarion w/o insurance", 5);
                await client.SendTextMessageAsync(chatid, "Your geolocation");
            }
            if (currentStep == 5)
            {
                if (messageObject.Message.Location != null)
                {
                    proposal.longtitude = messageObject.Message.Location.Longitude;
                    proposal.latitude = messageObject.Message.Location.Latitude;

                    string realAdres = await GeolocateHandler.GetAddressFromCordinat(proposal.longtitude, proposal.latitude);

                    proposal.RealAddress = realAdres;

                    await proposalRepository.Update(proposal);
                    await proposeHandler.ChangeProposeCount();

                    await adminPush.MessageRequisitionAsync(client, chatid);
                }
            }
        }

        private async Task CommandHandler_PaidRegistrationWithInsurance(long chatid, string message, MessageEventArgs messageObject = null)
        {
            int currentStep = await userRepository.GetCurrentActionStep(chatid);
            UserDTO user = await userRepository.FindById(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await proposalRepository.FindById(chatid);

            if (currentStep == 1)
            {
                user.FIO = message;
                await userRepository.Update(user);

                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 2);

                await client.SendTextMessageAsync(chatid, "Enter your tel.");
                return;
            }
            if (currentStep == 2)
            {
                await proposalRepository.Create(user);

                user.Phone = message;
                await userRepository.Update(user);

                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 3);
                await client.SendTextMessageAsync(chatid, "Enter your drone type");
                return;
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.Create(dron);

                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 4);
                await client.SendTextMessageAsync(chatid, "Enter insurance type");
                return;
            }
            if (currentStep == 4)
            {
                proposal.TypeOfInsurance = message;

                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 5);
                await client.SendTextMessageAsync(chatid, "Enter your address");
            }
            if (currentStep == 5)
            {
                proposal.Address = message;
                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 6);
                await client.SendTextMessageAsync(chatid, "Send your geolocation");
            }

            if (currentStep == 6)
            {
                if (messageObject.Message.Location != null)
                {
                    proposal.longtitude = messageObject.Message.Location.Longitude;
                    proposal.latitude = messageObject.Message.Location.Latitude;

                    string realAdres = await GeolocateHandler.GetAddressFromCordinat(proposal.longtitude, proposal.latitude);
                    proposal.RealAddress = realAdres;

                    await proposalRepository.Update(proposal);

                    await client.SendTextMessageAsync(chatid, "Waiting for payment"
                        , 0, false, false, 0, KeyboardHandler.Markup_After_Registration());

                    await proposeHandler.ChangeProposeCount();

                    await adminPush.MessageRequisitionAsync(client, chatid);
                    await client.SendTextMessageAsync(chatid, "Registration succeeded.");
                }
            }
        }
        #endregion
        #endregion

        public async Task BaseHandlerMessage(MessageEventArgs message, string text)
        {
            Console.WriteLine($"Sent : {chatid}\nText:{message.Message.Text}\n");
            chatid = message.Message.Chat.Id;
            string messageText = message.Message.Text;
            string action = await userRepository.GetCurrentActionName(chatid);

            await userRepository.AuthenticateUser(chatid);

            /*    if(await HubsHandler.IsChatActive(chatid))
            {
                long[] arrChatid = await HubsHandler.GetChatId(chatid);
                long chatIdRecive = arrChatid[0] == chatid ? arrChatid[1] : arrChatid[0];
                await client.SendTextMessageAsync(chatIdRecive, messageText);
                return;
            }*/

            await UserLogs.WriteLog(chatid, messageText);

            if (messageText == "/start")
            {
                await CommandHandler_Start(chatid);
            }

            if (messageText == "/chat")
            {
                await hubRepository.CreateDialog(chatid, 111111111);

                var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new[] {
                        new InlineKeyboardButton(){Text="Confirm", CallbackData="confirm"}
                    },
                    new[] {
                        new InlineKeyboardButton(){Text="Cancel", CallbackData="cancel"}
                    }
                });

                await client.SendTextMessageAsync(111111111, $"{message.Message.Chat.Username} wants to contact you", 0, false, false, 0, keyboard);
            }

            if (messageText == "/op")
            {

            }

            if (messageText == "Back")
            {
                await userRepository.ChangeAction(chatid, "NULL", 0);
                await CommandHandler_Start(chatid);

                return;
            }

            #region Paid registration for pilots

            if (messageText == "Paid registration with insurance")
            {
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name:", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                return;
            }

            if (messageText == "Paid registration w/o insurance")
            {
                await userRepository.ChangeAction(chatid, "Paid registration w/o insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                return;
            }
            #endregion

            #region Free registration for pilots

            if (messageText == "With insurance")
            {
                await userRepository.ChangeAction(chatid, "With insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                return;
            }
            if (messageText == "W/o insurance")
            {
                await userRepository.ChangeAction(chatid, "W/o insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
            }
            #endregion

            #region Corporate registration

            if (messageText == "Corporate")
            {
                await userRepository.ChangeAction(chatid, "Corporate registration", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());

                return;
            }
            if (messageText == "Private")
            {

            }
            #endregion

            if (messageText == "Client")
            {
                await client.SendTextMessageAsync(chatid, "There are several options for registration",
                    0, false, false, 0, KeyboardHandler.Markup_Start_Buisness_Mode());
            }

            if (messageText == "Full service (Payment)")
            {
                await client.SendTextMessageAsync(chatid, "There are several options with registration", 0, false, false, 0, KeyboardHandler.Markup_Start_Pilot_Payment_Mode());
            }

            if (messageText == "Pilot")
            {
                await client.SendTextMessageAsync(chatid, "You logged in as a pilot"
                    , 0, false, false, 0, KeyboardHandler.Markup_Start_Pilot_Mode());
            }

            if (action != null)
            {
                if (action == "Paid registration with insurance")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(chatid, messageText, message);
                    return;
                }
                if (action == "Paid registration w/o insurance")
                {
                    await CommandHandler_PaidRegistrationWithoutInsurance(chatid, messageText, message);
                    return;
                }
                if (action == "With insurance")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(chatid, messageText, message);
                    return;
                }
                if (action == "W/o insurance")
                {
                    await CommandHandler_PaidRegistrationWithoutInsurance(chatid, messageText, message);
                    return;
                }
                if (action == "Coprorate client")
                {
                    await CommandHandler_BuisnessRegistrationKorporativ(chatid, messageText, message);
                    return;
                }

                if (messageText == "Limited service(Free)")
                {
                    await client.SendTextMessageAsync(chatid, "There are several options with registration",
                        0, false, false, 0, KeyboardHandler.Markup_Start_Pilot_UnBuyer_Mode());
                }

            }
        }
    }
}
