using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using TeleDronBot.Repository;
using TeleDronBot.Services;
using Telegram.Bot;

namespace TeleDronBot.Bot.CommonHandler
{
    class AdminsPush
    {
        private CountProposeHandler propose;
        
        public AdminsPush()
        {
            propose = new CountProposeHandler();
        }

        public async Task MessageAboutRegistrationPilot(TelegramBotClient client, ServiceProvider provider, long chatid)
        {
            int countAdmin = await provider.adminService.CountAdmins();

            if (countAdmin == 0)
                return;

            List<long> admins = await provider.adminService.GetChatId();

            ProposalDTO proposal = await provider.proposalService.FindById(chatid);

            int numberOfPurpost = await propose.GetCount();
            UserDTO user = await provider.userService.FindUserByPredicate(i => i.ChatId == proposal.ChatId);
            
            if (user == null)
                throw new System.Exception("user is null");

            string message = $"Pilots: {propose.GetCount()}\n" +
                $"Pilot №{proposal.ChatId} is registered\n " +
                $"Name:{user.FIO}\n " +
                $"Tel.:{user.Phone}\n " +
                $"Insurance:{proposal.TypeOfInsurance}\n " +
                $"Address:{proposal.Address}\n " +
                $"Address, geolocation:{proposal.RealAddress}";

            admins.ForEach(async (item) =>
            {
                await client.SendTextMessageAsync(item, message);
            });
        }
    }
}
