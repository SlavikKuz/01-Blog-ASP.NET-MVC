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
    class BuisnessTaskRepository : BaseProviderImpementation<BuisnessTaskDTO>
    {
        public async ValueTask<List<int>> GetIdTasks(long? chatid = null, bool isBuisnessman = false)
        {
            if (isBuisnessman)
            {
                if (chatid == null)
                    throw new System.Exception("chatid cannot be null");
                return await db.BusinessTasks.Where(i => i.ChatId == chatid).Select(i => i.Id).ToListAsync();
            }
            return await db.BusinessTasks.Select(i => i.Id).ToListAsync();
        }

        public async ValueTask<int> MaxId(long chatid)
        {
            List<int> lstOfId = await GetIdTasks(chatid, true);
            lstOfId.Sort();
            return lstOfId[lstOfId.Count - 1];
        }
        
        public async ValueTask<int> MinId(long chatid)
        {
            List<int> lstOfId = await GetIdTasks(chatid, true);
            lstOfId.Sort();
            return lstOfId[0];
        }
        
        public async ValueTask<int> MaxId()
        {
            List<int> lstOfId = await db.BusinessTasks.Select(i => i.Id).ToListAsync();
            lstOfId.Sort();
            return lstOfId[lstOfId.Count - 1];
        }
        
        public async ValueTask<int> MinId()
        {
            List<int> lstOfId = await db.BusinessTasks.Select(i => i.Id).ToListAsync();
            lstOfId.Sort();
            return lstOfId[0];
        }
    }
}
