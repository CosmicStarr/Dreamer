
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Data.ClassesForInterfaces;
using Microsoft.Extensions.Configuration;

namespace NormStarr.EmailSenderServices
{
    public class MailJetSender : IMailJetEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSender(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public async Task SendEmail(string FromAddress, string ToAddress, string Subject, string Message)
        {
            var newMessage = new MailMessage(FromAddress, ToAddress, Subject,Message);

            using(var client = new SmtpClient(_configuration["STMP:Host"],int.Parse(_configuration["STMP:Port"]))
            {
                 Credentials = new NetworkCredential(_configuration["STMP:Username"],_configuration["STMP:Password"])
            })
            {
                 await client.SendMailAsync(newMessage);
            }
      
        }
    }
}