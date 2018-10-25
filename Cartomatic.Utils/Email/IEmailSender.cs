namespace Cartomatic.Utils.Email
{
    /// <summary>
    /// Email sender
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount"></param>
        /// <param name="emailTemplate"></param>
        /// <param name="recipient"></param>
        void Send(IEmailAccount emailAccount, IEmailTemplate emailTemplate, string recipient);
    }
}