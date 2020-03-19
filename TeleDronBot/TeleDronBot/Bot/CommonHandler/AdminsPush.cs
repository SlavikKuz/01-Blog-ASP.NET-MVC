using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using TeleDronBot.Repository;
using Telegram.Bot;

namespace TeleDronBot.Bot.CommonHandler
{
    class AdminsPush : RepositoryProvider
    {
        private CountProposeHandler propose;
        
        public AdminsPush()
        {
            propose = new CountProposeHandler();
        }

        public async Task MessageRequisitionAsync(TelegramBotClient client, long chatid)
        {
            int countAdmin = await adminRepository.CountAdmins();
            
            if (countAdmin == 0)
                return;
            
            List<long> admins = await adminRepository.GetChatId();

            ProposalDTO proposal = await proposalRepository.FindById(chatid);

            int numberOfPurpost = await propose.GetCount();
            UserDTO user = await userRepository.FirstElement(i => i.ChatId == proposal.ChatId);

            if (user == null)
                throw new Exception("user is null");

            string message = $"Application {propose.GetCount()}\n" +
                $"Pilot №{proposal.ChatId} is registered\n " +
                $"Name:{user.FIO}\n " +
                $"Tel.:{user.Phone}\n " +
                $"Insurance:{proposal.TypeOfInsurance}\n " +
                $"Address:{proposal.Address}\n " +
                $"Address, geolocation:{proposal.RealAddress}";

            StorageDTO storage = new StorageDTO();
            storage.Message = message;
            await storageRepository.Create(storage);
            
            foreach (long _chatid in admins)
            {
                await client.SendTextMessageAsync(_chatid, message);
            }
        }
    }
}
