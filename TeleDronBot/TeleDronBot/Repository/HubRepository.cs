using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Repository
{
    class HubRepository : BaseProviderImpementation<HubDTO>
    {

        static HubRepository()
        {

        }

        public async Task ConfirmDialog(string confirm, long CreaterChatId, long ReceiverChatId)
        {
            if (confirm == "Start")
            {
                HubDTO relatedHub = new HubDTO(ReceiverChatId, CreaterChatId);
                await Create(relatedHub);
                return;
            }
            else
            {
                HubDTO hub = await db.Hubs.FindAsync(CreaterChatId);
                await Delete(hub);
            }
        }

        public async Task CreateDialog(long CreaterChatId, long ReceiverChatId)
        {
            HubDTO hub = await db.Hubs.FindAsync(CreaterChatId);
            if (hub == null)
            {
                hub = new HubDTO(CreaterChatId, ReceiverChatId);
                await Create(hub);
                return;
            }
            hub.ChatIdReceiver = ReceiverChatId;
            await Update(hub);
        }
    }
}
