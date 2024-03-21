using Microsoft.AspNetCore.Hosting;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using WorkingWithMultipleTable_Prod.Models.ViewModel.Email;
using WorkingWithMultipleTable_Prod.Repository.Interface;

namespace WorkingWithMultipleTable_Prod.Repository.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmailSender(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<bool> SendEmailAsync(string email, string Subject, string message)
        {
            bool status = false;
            try
            {
                GetEmailSetting getEmailSetting = new GetEmailSetting()
                {
                    EmailKey = _configuration.GetValue<string>("AppSettings:EmailKey"),
                    From = _configuration.GetValue<string>("AppSettings:EmailSettings:From"),
                    SmtpServer = _configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer"),
                    Port = _configuration.GetValue<int>("AppSettings:EmailSettings:Port"),
                    EnableSSL = _configuration.GetValue<bool>("AppSettings:EmailSettings:EnableSSL")
                };

                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(getEmailSetting.From),
                    Subject = Subject,
                    Body = message,
                    BodyEncoding = System.Text.Encoding.ASCII,
                    IsBodyHtml  = true
                };
                mailMessage.To.Add(email);
                SmtpClient smtpClient = new SmtpClient(getEmailSetting.SmtpServer)
                {
                    Port = getEmailSetting.Port,
                    Credentials = new NetworkCredential(getEmailSetting.From, getEmailSetting.EmailKey),
                    EnableSsl = getEmailSetting.EnableSSL
                };
                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch (Exception)
            {
                status = false;
                
            }
            return status;
        }

        public string GetEmailBody(string? user, string? EmailTemplateName, string? callBackUrl, string? title)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Template/"+EmailTemplateName+".cshtml");
            string htmlString = System.IO.File.ReadAllText(path);
            htmlString = htmlString.Replace("{{title}}", title);
            htmlString = htmlString.Replace("{{Username}}", user);
            htmlString = htmlString.Replace("{{url}}", _configuration.GetValue<string>("URLS:ConfirmEmail") + user);
            htmlString = htmlString.Replace("{{callBackUrl}}", callBackUrl);
            return htmlString;
        }

      
    }
}
