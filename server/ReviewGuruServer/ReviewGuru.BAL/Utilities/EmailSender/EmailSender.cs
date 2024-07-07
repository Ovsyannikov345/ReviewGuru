using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReviewGuru.BLL.Utilities.Exceptions;

namespace ReviewGuru.BLL.Utilities.EmailSender
{
    public class EmailSender(IConfiguration configuration) : IEmailSender
    {
        private readonly IConfiguration _configuration = configuration;

        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_configuration["Email:Address"], _configuration["Email:Password"])
                };

                return client.SendMailAsync(
                    new MailMessage(from: _configuration["Email:From"]!,
                                    to: email,
                                    subject,
                                    message
                                    ));
            }
            catch (Exception)
            {
                throw new InternalServerErrorException("Error while sending email");
            }
        }
    }
}
