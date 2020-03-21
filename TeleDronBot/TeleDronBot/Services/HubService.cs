using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;

namespace TeleDronBot.Services
{
    class HubService : RepositoryProvider
    {
        public async Task ConfirmDialog(string confirm, long CreaterChatId, long ReceiverChatId)
        {
            if (confirm == "Start")
            {
                HubDTO reletedHub = new HubDTO(ReceiverChatId, CreaterChatId);
                await hubRepository.Create(reletedHub);
                return;
            }
            else
            {
                HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == CreaterChatId);
                await hubRepository.Delete(hub);
            }
        }

        public async Task CreateDialog(long CreaterChatId, long ReceiverChatId)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == CreaterChatId);
            if (hub == null)
            {
                hub = new HubDTO(CreaterChatId, ReceiverChatId);
                await hubRepository.Create(hub);
                return;
            }
            hub.ChatIdReceiver = ReceiverChatId;
            await hubRepository.Update(hub);
        }
    }
}
