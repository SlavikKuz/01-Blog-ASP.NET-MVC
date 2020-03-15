using System;
using System.Collections.Generic;
using System.Text;
using TeleDronBot.DTO;
using TeleDronBot.Interfaces;

namespace TeleDronBot.Repository
{
    class BotRepository : BaseRepository
    {
        GenericRepository<UserDTO> genericRepository;
        
        public BotRepository()
        {
            genericRepository = new GenericRepository<UserDTO>(db);
        }
    }
}
