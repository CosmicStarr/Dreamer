using System.Threading.Tasks;

namespace Data.ClassesForInterfaces
{
    public interface IMailJetEmailSender
    {
       Task SendEmail(string FromAddress, string ToAddress, string Subject, string Message);
    }
}