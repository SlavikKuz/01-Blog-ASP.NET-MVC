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
    class ManagerRepository : BaseProviderImpementation<ManagerDTO>
    {
        public async ValueTask<int> CountManager() =>
            await db.Managers.CountAsync();

        public async ValueTask<List<long>> ManagerId()
        {
            return await db.Managers.Select(i => i.ChatId).ToListAsync();
        }
    }
}
