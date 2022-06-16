using ServiceDesk.Data.ViewModels;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Interfaces
{
    public interface ISendMailService //where T : BaseEntity
    {
        Task Send(MailContent mailContent);

        Task SendEmail(string email, string subject, string htmlMessage);
    }
}