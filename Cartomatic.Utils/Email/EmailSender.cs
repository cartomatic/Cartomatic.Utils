﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Email
{
    /// <summary>
    /// Email sending functionality
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Sends an email to the recipient. Email is sent in a fire'n'forget manner.
        /// Note: in some scenarios fire'n'forget means the email may not eventually be sent at all.
        /// </summary>
        /// <param name="emailAccount">EmailAccount deails</param>
        /// <param name="emailTemplate">Email data to be sent out</param>
        /// <param name="recipient">Email of a recipient</param>
        public void Send(IEmailAccount emailAccount, IEmailTemplate emailTemplate, string recipient)
        {
            Task.Run(() =>
            {
                var mail = new MailMessage();

                mail.To.Add(recipient);

                mail.From = new MailAddress(emailAccount.Sender, !string.IsNullOrWhiteSpace(emailAccount.SenderName) ? emailAccount.SenderName : emailAccount.Sender);

                mail.Subject = emailTemplate.Title;
                mail.SubjectEncoding = Encoding.UTF8;

                mail.Body = emailTemplate.Body;
                mail.IsBodyHtml = emailTemplate.IsBodyHtml;
                mail.BodyEncoding = Encoding.UTF8;

                var smtp = new SmtpClient
                {
                    Host = emailAccount.SmtpHost,
                    Port = emailAccount.SmtpPort ?? 587,
                    Credentials = new System.Net.NetworkCredential(emailAccount.User, emailAccount.Pass),
                    EnableSsl = emailAccount.Ssl ?? false
                };

                var actionId = DateTime.Now.Ticks;

                try
                {

                    //Log($"{actionId} :: Attempting to send email to {recipient}; time start: {DateTime.Now.ToShortDateString()}-{DateTime.Now.ToShortTimeString()}");
                    smtp.Send(mail);
                    //Log($"{actionId} :: Email sent to {recipient}", $"Time taken in seconds: {new TimeSpan(DateTime.Now.Ticks - actionId).TotalSeconds}");
                }
                catch (Exception ex)
                {
                    var msgs = new List<string>
                    {
                        $"{actionId} :: Failed to send emails to {recipient}", $"Time taken in seconds: {new TimeSpan(DateTime.Now.Ticks - actionId).TotalSeconds}",
                        $"Sender details - host: {emailAccount.SmtpHost}, port: {emailAccount.SmtpPort}, sender: {emailAccount.Sender}, user: {emailAccount.User}, pass: {emailAccount.Pass}, ssl: {emailAccount.Ssl}",
                    };

                    var e = ex;
                    var tab = string.Empty;

                    while (e != null)
                    {

                        msgs.Add($"{tab}{e.Message}");
                        e = ex.InnerException;
                        tab += '\t';
                    }

                    //debug
                    //TODO - use proper logging utils!
                    //Log(
                    //    msgs.ToArray()
                    //);
                }
            });
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount"></param>
        /// <param name="emailTemplate"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public async Task SendAsync(IEmailAccount emailAccount, IEmailTemplate emailTemplate, string recipient)
        {
            await Task.Run(() =>
            {
                var mail = new MailMessage();

                mail.To.Add(recipient);

                mail.From = new MailAddress(emailAccount.Sender);

                mail.Subject = emailTemplate.Title;
                mail.SubjectEncoding = Encoding.UTF8;

                mail.Body = emailTemplate.Body;
                mail.IsBodyHtml = emailTemplate.IsBodyHtml;
                mail.BodyEncoding = Encoding.UTF8;

                var smtp = new SmtpClient
                {
                    Host = emailAccount.SmtpHost,
                    Port = emailAccount.SmtpPort ?? 587,
                    Credentials = new System.Net.NetworkCredential(emailAccount.User, emailAccount.Pass),
                    EnableSsl = emailAccount.Ssl ?? false
                };

                var actionId = DateTime.Now.Ticks;

                try
                {

                    //Log($"{actionId} :: Attempting to send email to {recipient}; time start: {DateTime.Now.ToShortDateString()}-{DateTime.Now.ToShortTimeString()}");
                    smtp.Send(mail);
                    //Log($"{actionId} :: Email sent to {recipient}", $"Time taken in seconds: {new TimeSpan(DateTime.Now.Ticks - actionId).TotalSeconds}");
                }
                catch (Exception ex)
                {
                    var msgs = new List<string>
                    {
                        $"{actionId} :: Failed to send emails to {recipient}", $"Time taken in seconds: {new TimeSpan(DateTime.Now.Ticks - actionId).TotalSeconds}",
                        $"Sender details - host: {emailAccount.SmtpHost}, port: {emailAccount.SmtpPort}, sender: {emailAccount.Sender}, user: {emailAccount.User}, pass: {emailAccount.Pass}, ssl: {emailAccount.Ssl}",
                    };

                    var e = ex;
                    var tab = string.Empty;

                    while (e != null)
                    {

                        msgs.Add($"{tab}{e.Message}");
                        e = ex.InnerException;
                        tab += '\t';
                    }

                    //debug
                    //TODO - use proper logging utils!
                    //Log(
                    //    msgs.ToArray()
                    //);
                }
            });
        }
    }
}
