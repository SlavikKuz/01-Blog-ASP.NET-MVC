using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Repository
{
    class BuisnessTaskRepository : BaseProviderImpementation<BuisnessTaskDTO>
    {
        public int MaxId() =>
            db.BusinessTasks.Max(i => i.Id);
        public int MinId() =>
            db.BusinessTasks.Min(i => i.Id);
    }
}
