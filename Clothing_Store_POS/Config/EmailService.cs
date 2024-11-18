using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Config
{
    public class EmailService
    {
        private readonly string stmpServer;
        private readonly int smtpPort;
        private readonly string sender;
        private readonly string password;

        public EmailService()
        {
            this.stmpServer = "smtp.gmail.com";
            this.smtpPort = 587;
            this.sender = "insideafish@gmail.com";
            this.password = "gusm wafl bsth qzrd";
        }

        public void SendEmail(List<string> recipientMails, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Clothing Store", sender));
            foreach (string recipient in recipientMails)
            {
                message.To.Add(new MailboxAddress("Customer", recipient));
            }
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = body
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(stmpServer, smtpPort, false);
                    client.Authenticate(sender, password);

                    client.Send(message);
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}
