using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Services
{
    class sosTableService : RepositoryProvider
    {
        public async Task Create(SosDTO sos)
        {
            await sosTableRepository.Create(sos);
        }
        
        public async Task Update(SosDTO sos)
        {
            await sosTableRepository.Update(sos);
        }
        
        public async ValueTask<SosDTO> FindById(long chatid)
        {
            SosDTO sos = await sosTableRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            return sos;
        }
    }
}
