using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Repository
{
    class ProposalRepository : BaseProviderImpementation<ProposalDTO>
    {
        public async Task RemoveRange(IEnumerable<ProposalDTO> lst)
        {
            db.RemoveRange(lst);
            await db.SaveChangesAsync();
        }

        public async Task Create(UserDTO user)
        {            
            if (user == null)
                throw new NullReferenceException("User cannot be null");
            
            ProposalDTO proposal = await db.proposalsDTO.AsNoTracking().FirstOrDefaultAsync(i => i.ChatId == user.ChatId);
            
            if (proposal != null)
                return;

            proposal = new ProposalDTO()
            {
                ChatId = user.ChatId
            };

            await base.Create(proposal);
        }
    }
}
