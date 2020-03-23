using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.Bot;
using TeleDronBot.DTO;
using TeleDronBot.Geolocation;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TeleDronBot.PilotCommands
{
    class RegistrationPilotCommand : BaseCommand
    {
        public RegistrationPilotCommand(TelegramBotClient client, MainProvider provider) 
            : base(client, provider) { }

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
                await provider.dronService.Create(dron);
                await provider.userService.ChangeAction(chatid, "Paid registarion w/o insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter your address");
                return;
            }
            
            if (currentStep == 4)
            {
                await provider.proposalService.Create(user);
                proposal.Address = message;
                await provider.proposalService.Update(proposal);
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

                    await provider.proposalService.Update(proposal);
                    await provider.proposeHandler.ChangeProposeCount();

                    user.PilotPrivilage = 1;

                    await provider.userService.Update(user);
                    await provider.adminPush.MessageRequisitionAsync(client, provider, chatid);
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
                await provider.proposalService.Create(user);

                user.Phone = message;

                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter your drone type");
                return;
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await provider.dronService.Create(dron);

                await provider.userService.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter insurance type");
                return;
            }
            if (currentStep == 4)
            {
                proposal.TypeOfInsurance = message;

                await provider.proposalService.Update(proposal);
                await provider.userService.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Enter your address");
                return;
            }
            if (currentStep == 5)
            {
                proposal.Address = message;
                await provider.proposalService.Update(proposal);
                await provider.userService.ChangeAction(chatid, "Paid registration with insurance", ++currentStep);
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

                    await provider.proposalService.Update(proposal);

                    await client.SendTextMessageAsync(chatid, "Waiting for payment"
                        , 0, false, false, 0, KeyboardHandler.PilotWithSubscribe_Markup());

                    await provider.proposeHandler.ChangeProposeCount();

                    user.PilotPrivilage = 2;

                    await provider.userService.Update(user);
                    await provider.adminPush.MessageRequisitionAsync(client, provider, chatid);

                    await client.SendTextMessageAsync(chatid, "Registration succeeded.");
                }
            }
        }
    }
}
