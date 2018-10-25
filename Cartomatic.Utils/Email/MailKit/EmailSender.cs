using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Cartomatic.Utils.Email.MailKit
{
    /// <summary>
    /// Email sending functionality
    /// </summary>
    public class EmailSender: IEmailSender
    {
        /// <summary>
        /// Sends an email to the recipient. Email is sent in a fire'n'forget manner.
        /// Note: in some scenarios fire'n'forget means the email may not eventually be sent at all.
        /// Uses Mailkit as an smtp client
        /// </summary>
        /// <param name="emailAccount">EmailAccount deails</param>
        /// <param name="emailTemplate">Email data to be sent out</param>
        /// <param name="recipient">Email of a recipient</param>
        public void Send(IEmailAccount emailAccount, IEmailTemplate emailTemplate, string recipient)
        {
            Task.Run(() =>
            {
                var msg = new MimeMessage();
                msg.To.Add(new MailboxAddress(recipient));
                msg.From.Add(new MailboxAddress(emailAccount.Sender));

                msg.Subject = emailTemplate.Title;
                msg.Body = new TextPart(emailTemplate.IsBodyHtml ? TextFormat.Html : TextFormat.Plain)
                {
                    Text = emailTemplate.Body
                };

                using (var emailClient = new SmtpClient())
                {
                    try
                    {
                        //emailClient.Connect(emailAccount.SmtpHost, emailAccount.SmtpPort ?? 587, true);
                        //looks like outlook mailkit does not like the enforce secure connection when talking to outlook online
                        //i guess outlook will enforce the tsl anyway
                        emailClient.Connect(emailAccount.SmtpHost, emailAccount.SmtpPort ?? 587);

                        //not using oauth, so remove it!
                        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                        emailClient.Authenticate(emailAccount.User, emailAccount.Pass);

                        emailClient.Send(msg);

                        emailClient.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        //ignore
                    }
                    
                }
            });
        }
    }
}
