using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeleDronBot.Base.BaseClass;
using TeleDronBot.DTO;
using Telegram.Bot;

namespace TeleDronBot.Bot.CommonHandler
{
    class ManagerPush : RepositoryProvider
    {
        public async Task SendMessage(TelegramBotClient client, long chatid)
        {
            int countManager = await managerRepository.CountManager();
            
            if (countManager == 0)
                return;
            
            List<long> chatIds = await managerRepository.ManagerId();
            
            UserDTO user = await userRepository.FindById(chatid);
            
            if (user == null)
                throw new NullReferenceException("User cannot be null");
            
            string message = $"Full name:{user.FIO}\n " +
                $"Tel.:{user.Phone}";
            
            foreach (var i in chatIds)
                await client.SendTextMessageAsync(i, message);
            
            return;
        }
    }
}
