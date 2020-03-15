using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.DTO;
using TeleDronBot.Repository;
using Telegram.Bot;

namespace TeleDronBot.Bot.CommonHandler
{
    class AdminsPush : BaseRepository
    {

        public async Task MessageRequisitionAsync(long chatid)
        {
            int countAdmin = await db.Admins.CountAsync();
            if (countAdmin == 0)
                return;
            long[] chatids = new long[countAdmin];

            ProposalDTO proposal = await proposalRepository.GetCurrentProposal(chatid);

            int numberOfPurpost = await CountProposeHandler.GetCount();
            UserDTO user = await db.Users.FirstOrDefaultAsync(i => i.ChatId == proposal.ChatId);

            if (user == null)
                throw new Exception("user is null");

            string message = $"Pilot №{proposal.ChatId} registered\n" +
                $"Name:{user.FIO}\n " +
                $"Tel.:{user.Phone}\n " +
                $"Insurance:{proposal.TypeOfInsurance}\n " +
                $"Address:{proposal.Address}\n " +
                $"Address, geolocation:{proposal.RealAddress}";

            foreach (long _chatid in chatids)
            {
                await client.SendTextMessageAsync(_chatid, message);
            }
        }
    }
}
