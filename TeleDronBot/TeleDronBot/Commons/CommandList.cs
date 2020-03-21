using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TeleDronBot.Commons
{
    class CommandList
    {
        public static List<string> GetCommands()
        {
            List<string> result = new List<string>();
            string line;
            using (StreamReader reader = new StreamReader("Commands.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                    result.Add(line);
            }
            return result;
        }
        
        public static List<string> RegistrationPilotCommandList()
        {
            List<string> result = new List<string>();
            result.Add("Full functionality paid");
            result.Add("Full functionality for a fee");
            result.Add("With insurance");
            result.Add("Without insurance");
            result.Add("Paid registration with insurance");
            result.Add("Paid registration without insurance");
            return result;
        }
        
        public static List<string> RegistrationBuisnessCommandList()
        {
            List<string> result = new List<string>();
            result.Add("Private client");
            result.Add("Corporate");
            return result;
        }
    }
}
