using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils.Email
{
    public interface IEmailManager
    {
        void SendSingleEmail(string receiverAddress, string message, string subject);
        void SendBulkEmail(string[] receiverAddress, string message, string subject);
        string GetCreateInstanceEmailTemplate(string adminName, string instanceName, string emailLink);
        void SendSingleEmailWithAttachment(string receiverAddress, string message, string subject, string fileName,
            string fileContent, string type);
        string GetConfirmEmailTemplate(string emailLink, string email);
        string GetResetPasswordEmailTemplate(string emailLink, string email);
    }
}
