using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot.CommonHandler;
using TeleDronBot.Commons;
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
    class MessageHandler : IMessageHandler
    {
        long chatid;
        TelegramBotClient client;
        MainProvider provider;

        public MessageHandler(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }

        #region PrivateHandlers

        #region BusinessRegistration
        
        private async Task CommandHandler_BuisnessRegistrationKorporativ(long chatid, string message, MessageEventArgs messageObject)
        {
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            UserDTO user = await provider.userService.FindById(chatid);
            DronDTO dron = new DronDTO();

            if (currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Corporate client registration", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter your telephone number");
                return;
            }

            if (currentStep == 2)
            {
                user.Phone = message;
                user.BusinessPrivilage = 1;
                await provider.userService.Update(user);
                await client.SendTextMessageAsync(chatid, "Registation succeeded");
                await provider.managerPush.SendMessage(client, chatid);
                return;
            }
        }
        #endregion

        private async Task CommandHandler_Start(long chatid)
        {
            await client.SendTextMessageAsync(chatid, "Sample text", 0, false, false, 0, KeyboardHandler.Markup_Start_AfterChange());
        }

        #region PaymentRegistration

        private async Task CommandHandler_PaidRegistrationWithoutInsurance(UserDTO user, string message, MessageEventArgs messageObject)
        {
            long chatid = user.ChatId;
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);

            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await provider.proposalService.FindById(chatid);

            if (currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Paid registarion w/o insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter mobile phone number");
                return;
            }
            if (currentStep == 2)
            {
                user.Phone = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Paid registarion w/o insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter drone type");
                return;
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.Create(dron);
                await provider.userService.ChangeAction(chatid, "Paid registarion w/o insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter your address");
                return;
            }
            if (currentStep == 4)
            {
                await proposalRepository.Create(user);
                proposal.Address = message;
                await provider.userService.Update(proposal);
                await provider.userService.ChangeAction(chatid, "Paid registarion w/o insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Your geolocation");
                return;
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

                    user.PilotPrivilage = 1;
                    await userRepository.Update(user);

                    await adminPush.MessageRequisitionAsync(client, chatid);
                }
            }
        }

        private async Task CommandHandler_PaidRegistrationWithInsurance(UserDTO user, string message, MessageEventArgs messageObject = null)
        {
            long chatid = user.ChatId;
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);

            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await provider.proposalService.FindById(chatid);

            if (currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);

                await provider.userService.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);

                await client.SendTextMessageAsync(chatid, "Enter your tel.");
                return;
            }
            if (currentStep == 2)
            {
                await proposalRepository.Create(user);

                user.Phone = message;
                await userRepository.Update(user);

                await userRepository.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter your drone type");
                return;
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.Create(dron);

                await userRepository.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter insurance type");
                return;
            }
            if (currentStep == 4)
            {
                proposal.TypeOfInsurance = message;

                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter your address");
                return;
            }
            if (currentStep == 5)
            {
                proposal.Address = message;
                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Send your geolocation");
                return;
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
                        , 0, false, false, 0, KeyboardHandler.PilotWithSubscribe_Markup());

                    await proposeHandler.ChangeProposeCount();

                    user.PilotPrivilage = 2;
                    await userRepository.Update(user);

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
            string action = await provider.userService.GetCurrentActionName(chatid);
            UserDTO user = await provider.userService.FindById(chatid);

            await provider.userService.AuthenticateUser(chatid);

            await UserLogs.WriteLog(chatid, messageText);

            bool isRegistration = await provider.userService.IsUserRegistration(chatid);
            
            if (messageText == "Back")
            {
                await provider.userService.ChangeAction(chatid, "NULL", 0);
                await CommandHandler_Start(chatid);
                return;
            }

            if (messageText == "/start")
            {
                await CommandHandler_Start(chatid);
                return;
            }

            if (CommandList.RegistrationPilotCommandList().Contains(messageText) && user.PilotPrivilage != 0)
            {
                await client.SendTextMessageAsync(chatid, "You are already registered", 0, false, false, 0, KeyboardHandler.ChangeKeyBoardPilot(user.PilotPrivilage));
                return;
            }

            if (CommandList.RegistrationBuisnessCommandList().Contains(messageText) && user.BusinessPrivilage != 0)
            {
                await client.SendTextMessageAsync(chatid, "You are already registered", 0, false, false, 0, KeyboardHandler.Markup_BuisnessmanMenu());
                return;
            }
            
            if (messageText == "Pilot")
            {
                if (user.PilotPrivilage == 0)
                {
                    await client.SendTextMessageAsync(chatid, "You logged in as a pilot"
                        , 0, false, false, 0, KeyboardHandler.Markup_Start_Pilot_Mode());
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "You logged in as a pilot",
                        0, false, false, 0, KeyboardHandler.ChangeKeyBoardPilot(user.PilotPrivilage));
                }
            }

            if (messageText == "Full access paid")
            {
                await client.SendTextMessageAsync(chatid, "There are several options for registration", 
                    0, false, false, 0, KeyboardHandler.Markup_Start_Pilot_Payment_Mode());
            }

            if (messageText == "Limited access free")
            {
                await client.SendTextMessageAsync(chatid, "There are several options for registration",
                    0, false, false, 0, KeyboardHandler.Markup_Start_Pilot_UnBuyer_Mode());
            }

            #region Paid registration for pilots

            if (messageText == "With insurance")
            {
                await provider.userService.ChangeAction(chatid, "With insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name:", 
                    0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                return;
            }

            if (messageText == "W/o insurance")
            {
                await provider.userService.ChangeAction(chatid, "W/o insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 
                    0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
            }

            if (messageText == "Paid registration with insurance")
            {
                await provider.userService.ChangeAction(chatid, "Paid registration with insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 
                    0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                return;
            }
            
            if (messageText == "Paid registration w/o insurance")
            {
                await provider.userService.ChangeAction(chatid, "Paid registration w/o insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 
                    0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                return;
            }

            #endregion
            #region Free registration for pilots

            #endregion

            #region Corporate registration

            if (messageText == "Corporate")
            {
                await provider.userService.ChangeAction(chatid, "Corporate registration", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());

                return;
            }
            if (messageText == "Private")
            {

            }
            #endregion

            if (messageText == "Client")
            {
                if (user.BusinessPrivilage == 0)
                {
                    await client.SendTextMessageAsync(chatid, "There are several options for registration.",
                        0, false, false, 0, KeyboardHandler.Markup_Start_Buisness_Mode());
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "You are already registered.",
                        0, false, false, 0, KeyboardHandler.Markup_BuisnessmanMenu());
                }
                return;
            }

            if (action != null)
            {
                if (action == "Paid registration with insurance")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(user, messageText, message);
                    return;
                }
                if (action == "Paid registration w/o insurance")
                {
                    await CommandHandler_PaidRegistrationWithoutInsurance(user, messageText, message);
                    return;
                }
                if (action == "With insurance")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(user, messageText, message);
                    return;
                }
                if (action == "W/o insurance")
                {
                    await CommandHandler_PaidRegistrationWithoutInsurance(user, messageText, message);
                    return;
                }
                if (action == "Coprorate client")
                {
                    await CommandHandler_BuisnessRegistrationKorporativ(chatid, messageText, message);
                    return;
                }
            }
        }
    }
}
