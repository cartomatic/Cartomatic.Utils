using System.Collections.Generic;

namespace Cartomatic.Utils.Email
{
    /// <summary>
    /// Enforces Email template 
    /// </summary>
    public interface IEmailTemplate
    {
        /// <summary>
        /// Email template as fed to email sender
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// email body
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// whether or not body is html
        /// </summary>
        bool IsBodyHtml { get; set; }

        /// <summary>
        /// Prepares the template based on a collection of token/value to be applied
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        IEmailTemplate Prepare(IDictionary<string, object> tokens);
    }
}