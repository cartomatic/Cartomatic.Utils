using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cartomatic.Utils.Email
{
    /// <summary>
    /// Email account details needed to connect to the smtp server
    /// </summary>
    public class EmailAccount : IEmailAccount
    {
        /// <summary>
        /// Sender email - from 
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Smtp host to be used to connect to the smtp server
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// Smtp port
        /// </summary>
        public int? SmtpPort { get; set; }

        /// <summary>
        /// Email account user used to login to the smtp server
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Password used to login to the smtp server
        /// </summary>
        public string Pass { get; set; }

        /// <summary>
        /// Whether or not the communication should be encrypted
        /// </summary>
        public bool? Ssl { get; set; }

        /// <summary>
        /// Whether or not email account seems complete - can send out emails
        /// </summary>
        /// <returns></returns>
        public bool SeemsComplete()
        {
            return !string.IsNullOrWhiteSpace(Sender) &&
                   !string.IsNullOrWhiteSpace(SmtpHost) &&
                   !string.IsNullOrWhiteSpace(User) &&
                   !string.IsNullOrWhiteSpace(Pass);
        }

        /// <summary>
        /// Creates instance from a json string; throws if json is not valid
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static EmailAccount FromJson(string json)
        {
            return JsonConvert.DeserializeObject<EmailAccount>(json);
        }

        /// <summary>
        /// serializes object to url query string
        /// </summary>
        /// <returns></returns>
        public string ToQuery()
        {
            return string.Join("&", new[]
            {
                $"{nameof(Sender)}={Sender}",
                $"{nameof(SmtpHost)}={SmtpHost}",
                $"{nameof(SmtpPort)}={SmtpPort}",
                $"{nameof(User)}={User}",
                $"{nameof(Pass)}={Pass}",
                $"{nameof(Ssl)}={Ssl}"
            });
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { nameof(Sender), Sender },
                { nameof(SmtpHost), SmtpHost },
                { nameof(SmtpPort), SmtpPort },
                { nameof(User), User },
                { nameof(Pass), Pass },
                { nameof(Ssl), Ssl }
            };
        }
    }
}
