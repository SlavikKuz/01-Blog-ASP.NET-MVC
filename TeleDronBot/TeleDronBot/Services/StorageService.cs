using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Services
{
    class StorageService : RepositoryProvider
    {
        public async Task Create(StorageDTO storage)
        {
            await storageRepository.Create(storage);
        }
    }
}
