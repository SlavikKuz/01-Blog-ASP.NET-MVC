using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Services
{
    class ProposalService : RepositoryProvider
    {
        public async ValueTask<ProposalDTO> FindById(long chatid)
        {
            return await proposalRepository.FindById(chatid);
        }
    }
}
