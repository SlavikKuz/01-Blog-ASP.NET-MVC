using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TeleDronBot.Commons
{
    class Email
    {
        public static async Task SendEmail(string message)
        {
            MailAddress from = new MailAddress("xxxx", "Slavik");
            MailAddress to = new MailAddress("stest@test.com");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Test";
            m.Body = message;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("xxxx", "xxxxx");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
}
