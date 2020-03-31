using System;
using System.Linq;
using TeleDronBot.Bot;
using TeleDronBot.Geolocation;
using TeleDronBot.Logs;

namespace TeleDronBot
{
    class Program
    {
        static async void ReadLogs(long chatid)
        {
            if (UserLogs.GetLogs(chatid) != null)
            {
                UserLogs.GetLogs(chatid).ToList().ForEach((s) => Console.WriteLine(s));
            }
        }
       

        static void Main(string[] args)
        {
            Console.WriteLine("Press 1 to start TeleBot");
            Console.WriteLine("Press 2 to see user logs");
            if (Convert.ToInt32(Console.ReadLine()) == 1)
            {
                Console.WriteLine("server running");
                StartBot bot = new StartBot();
                bot.BotRun();
            }
            if (Convert.ToInt32(Console.ReadLine()) == 2)
            {
                Console.WriteLine("Enter сhatid");
                long chatid = Convert.ToInt64(Console.ReadLine());
                ReadLogs(chatid);
            }
            Console.ReadLine();
        }
    }
}
