using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Services
{
    class RegionService : RepositoryProvider
    {
        public async Task Create(string name)
        {
            RegionsDTO region = await regionsRepository.Get().FirstOrDefaultAsync(i => i.Name == name);
            if (region == null)
            {
                region = new RegionsDTO();
                region.Name = name;
                await regionsRepository.Create(region);
            }
        }
        public async ValueTask<List<string>> GetAllRegions()
        {
            return await regionsRepository.Get().Select(i => i.Name).ToListAsync();
        }
    }
}
