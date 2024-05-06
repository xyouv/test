using System.Net;
using System.Net.Mail;

namespace PhoneManagement.Service {

    public interface IEmailService {
        void sendMail(SendMailModel model);
    }
    public class EmailService : IEmailService {
        public EmailService() {
        }

        public void sendMail(SendMailModel model) {
            try {
                MailMessage mailMessage = new MailMessage() {
                    Subject = model.subject,
                    Body = model.content,
                    IsBodyHtml = false
                };
                mailMessage.From = new MailAddress(EmailSettingModel.Instance.FromEmailAddress, EmailSettingModel.Instance.FromDisplayName);
                mailMessage.To.Add(model.recieveAddress);
                var smtp = new SmtpClient() {
                    EnableSsl = EmailSettingModel.Instance.Smtp.EnableSsl,
                    Host = EmailSettingModel.Instance.Smtp.Host,
                    Port = EmailSettingModel.Instance.Smtp.Port,
                };
                var network = new NetworkCredential(EmailSettingModel.Instance.Smtp.EmailAddress, EmailSettingModel.Instance.Smtp.Password);
                smtp.Credentials = network;
                smtp.Send(mailMessage);
            } catch (Exception e) {
                throw;
            }
        }
    }
}
