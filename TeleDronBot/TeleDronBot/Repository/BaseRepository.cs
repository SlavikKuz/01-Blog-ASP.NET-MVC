using System;
using System.Collections.Generic;
using System.Text;
using TeleDronBot.DTO;

namespace TeleDronBot.Repository
{
    class BaseRepository
    {
        protected ApplicationContext db;
        public BaseRepository()
        {
            db = new ApplicationContext();
        }
    }
}
