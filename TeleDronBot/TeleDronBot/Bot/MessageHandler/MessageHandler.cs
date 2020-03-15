using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    class MessageHandler : IMessageHandler
    {
        long chatid;
        TelegramBotClient client;
        ApplicationContext db;
        #region repository
        AdminsPush adminsPush;
        UserRepository userRepository;
        GenericRepository<UserDTO> genericUserRepository;
        DronRepository dronRepository;
        BotRepository botRepository;
        HubRepository hubRepository;
        AdminRepository adminRepository;
        ProposalRepository proposalRepository;
        #endregion
        public MessageHandler(TelegramBotClient client, ApplicationContext context)
        {
            this.client = client;
            db = context;
            userRepository = new UserRepository();
            genericUserRepository = new GenericRepository<UserDTO>(db);
            dronRepository = new DronRepository();
            botRepository = new BotRepository();
            hubRepository = new HubRepository();
            adminRepository = new AdminRepository();
            proposalRepository = new ProposalRepository();
            adminsPush = new AdminsPush(client);
        }

        #region PrivateHandlers
        private async Task CommandHandler_Start(long chatid)
        {
            await client.SendTextMessageAsync(chatid, "Sample text", 0, false, false, 0, KeyboardHandler.Markup_Start());
        }
        private async Task CommandHandler_PaidRegistrationWithInsurance(long chatid, string message, MessageEventArgs messageObject = null)
        {
            int currentStep = userRepository.GetCurrentActionStep(chatid);
            UserDTO user = await genericUserRepository.FindById(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await proposalRepository.GetCurrentProposal(chatid);
            
            if (currentStep == 1)
            {
                user.FIO = message;

                await genericUserRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 2);
                //await client.SendTextMessageAsync(chatid, "Enter your drone type");
                await client.SendTextMessageAsync(chatid, "Enter your tel.");
                return;
            }
            if (currentStep == 2)
            {
                await proposalRepository.CreateProposal(chatid);
                
                user.Phone = message;
                await genericUserRepository.Update(user);
                
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 3);
                await client.SendTextMessageAsync(chatid, "Enter your drone type");
                return;
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.CreateDron(dron);

                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 4);
                await client.SendTextMessageAsync(chatid, "Enter insurance type");
                return;
            }
            if (currentStep == 4)
            {
                proposal.TypeOfInsurance = message;

                await proposalRepository.UpdateProposal(proposal);
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 5);
                await client.SendTextMessageAsync(chatid, "Enter your address");
            }
            if (currentStep == 5)
            {
                proposal.Address = message;
                await proposalRepository.UpdateProposal(proposal);
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

                    await proposalRepository.UpdateProposal(proposal);
                    await genericUserRepository.Update(user);

                    await client.SendTextMessageAsync(chatid, "Waiting for payment"
                        , 0, false, false, 0, KeyboardHandler.Markup_After_Registration());
                    await adminsPush.MessageRequisitionAsync(chatid);
                }
            }
        }
        #endregion

        public async Task BaseHandlerMessage(MessageEventArgs message, string text)
        {
            Console.WriteLine($"Sent : {chatid}\nText:{message.Message.Text}\n");
            chatid = message.Message.Chat.Id;
            string messageText = message.Message.Text;

            await userRepository.AuthenticateUser(chatid);

            if (await HubsHandler.IsChatActive(chatid))
            {
                long[] arrChatid = await HubsHandler.GetChatId(chatid);

                long chatIdRecive = arrChatid[0] == chatid ? arrChatid[1] : arrChatid[0];
                await client.SendTextMessageAsync(chatIdRecive, messageText);

                return;
            }

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
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Confirm",CallbackData="confirm"}
                    },
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Cancel",CallbackData="cancel"}
                    }
                });
                await client.SendTextMessageAsync(111111111, $"{message.Message.Chat.Username} wants to contact you", 0, false, false, 0, keyboard);
            }

            if (messageText == "/op")
            {

            }

            if (messageText == "Back")
            {
                await proposalRepository.DeleteNotFillProposalAsync(chatid);
                await userRepository.ChangeAction(chatid, "NULL", 0);
                await CommandHandler_Start(chatid);

                return;
            }

            if (messageText == "Paid registration with insurance")
            {
                await userRepository.ChangeAction(chatid, "Paid registration with insurance", 1);
                await client.SendTextMessageAsync(chatid, "Enter your name:", 0, false, false, 0, KeyboardHandler.Markup_Back_From_First_Action());
                return;
            }

            if (messageText == "Buyer")
            {
                await client.SendTextMessageAsync(chatid, "You logged in as a buyer"
                    , 0, false, false, 0, KeyboardHandler.Markup_Start_BuyerMode());
            }


            if (userRepository.IsUserInAction(chatid))
            {
                string action = userRepository.GetCurrentActionName(chatid);
                if (action == "Paid registration with insurance")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(chatid, messageText, message);
                }
                return;
            }

        }
    }
}
