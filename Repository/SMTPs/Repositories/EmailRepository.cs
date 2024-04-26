using Microsoft.Extensions.Configuration;
using Repository.SMTPs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Repository.DBContext;

namespace Repository.SMTPs.Repositories
{
    public class EmailRepository
    {

        private ZRDbContext _dbContext;
        public EmailRepository(ZRDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        private Email GetEmailProperty()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                  .SetBasePath(Directory.GetCurrentDirectory())
                                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            return new Email()
            {
                Host = configuration.GetSection("Verification:Email:Host").Value,
                Port = int.Parse(configuration.GetSection("Verification:Email:Port").Value),
                SystemName = configuration.GetSection("Verification:Email:SystemName").Value,
                Sender = configuration.GetSection("Verification:Email:Sender").Value,
                Password = configuration.GetSection("Verification:Email:Password").Value,
            };
        }

        public string GetMessageForgotAccount(string receiverEmail, string password, string messageBody)
        {
            Email email = GetEmailProperty();
            string emailBody = "";
            string htmlTableDivStart = "<table style=\"border-collapse: collapse; width: 50%; margin: 20px auto; border: 1px solid #ddd;\">";
            string htmlTableDivEnd = "</div>";

            string htmlTable = String.Format(@"
                                        <table>
                                    <tr>
                                      <th>Password</th>
                                      <td>{0}</td>
                                    </tr>
                                       </table>
                                  ", password);

            string htmlParentDivStart = "<div style=\"font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2\">";
            string htmlParentDivEnd = "</div>";
            string htmlMainDivStart = "<div style=\"margin:50px auto;width:70%;padding:20px 0\">";
            string htmlMainDivEnd = "</div>";
            string htmlSystemNameDivStart = "<div style=\"border-bottom:1px solid #eee\">";
            string htmlSystemNameDivEnd = "</div";
            string htmlSystemNameSpanStart = "<span style=\"font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600\">";
            string htmlSystemNameSpanEnd = "</span>";
            string htmlHeaderBodyStart = "<p style=\"font-size:1.1em\">";
            string htmlHeaderBodyEnd = "</p>";
            string htmlBodyStart = "<p>";
            string htmlBodyEnd = "</p>";
            string htmlFooterBodyStart = "<p style=\"font-size:0.9em;\">";
            string htmlBreakLine = "<br />";
            string htmlFooterBodyEnd = "</p>";

            emailBody += htmlParentDivStart;
            emailBody += htmlMainDivStart;

            emailBody += htmlSystemNameDivStart + htmlSystemNameSpanStart
                        + email.SystemName + htmlSystemNameSpanEnd + htmlSystemNameDivEnd
                        + htmlBreakLine;

            emailBody += htmlHeaderBodyStart + $"Hi {receiverEmail}," + htmlHeaderBodyEnd;
            emailBody += htmlBodyStart + messageBody + htmlBodyEnd;
            emailBody += htmlTableDivStart + htmlTable + htmlTableDivEnd;
            emailBody += htmlFooterBodyStart + "Regards," + htmlBreakLine + email.SystemName + htmlFooterBodyEnd;
            emailBody += htmlMainDivEnd;
            emailBody += htmlParentDivEnd;

            return emailBody;
        }

        public async Task SendAccountToEmailAsync(string reciever, string message)
        {
            try
            {
                Email email = GetEmailProperty();
                string subject = $"Password in Zens Restaurant System";
                SmtpClient smtpClient = new SmtpClient(email.Host, email.Port);
                smtpClient.Credentials = new NetworkCredential(email.Sender, email.Password);
                smtpClient.EnableSsl = true;
                MailMessage mailMessage = new MailMessage(email.Sender, reciever, subject, message);
                mailMessage.IsBodyHtml = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (AggregateException ex)
            {
                throw new AggregateException(ex.InnerExceptions);
            }
        }
    }
}
