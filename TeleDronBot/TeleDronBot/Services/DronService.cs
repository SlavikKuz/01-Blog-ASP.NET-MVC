using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Services
{
    class DronService : RepositoryProvider
    {
        public async Task Create(DronDTO dron)
        {
            await dronRepository.Create(dron);
        }
    }
}
