using System;
using System.Collections.Generic;
using System.Text;
using TeleDronBot.DTO;

namespace TeleDronBot.Base.BaseClass
{
    class DbProvider<T>
    {
        private ApplicationContext _db;
        
        protected ApplicationContext db
        {
            get
            {
                if (_db == null)
                    _db = new ApplicationContext();
                return _db;
            }
        }
    }
}
