using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace CA.Infrastucture.Helpers
{
    public class MailHelper
    {
        public async Task SendEmail(string emailTo, string subject, string bodyText)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Crypto Trading", "komulativ11@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", emailTo));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = bodyText
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("crytpotradingautomation@gmail.com", "crytpotradingautomation111");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
  
    }
}
