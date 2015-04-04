using System.Net.Mail;
using Creek.UI.ExceptionReporter.Core;

namespace Creek.UI.ExceptionReporter.Mail
{
    internal class MailSender
    {
        #region Delegates

        public delegate void CompletedMethodDelegate(bool success);

        #endregion

        private readonly ExceptionReportInfo _reportInfo;

        internal MailSender(ExceptionReportInfo reportInfo)
        {
            _reportInfo = reportInfo;
        }

        public string EmailSubject
        {
            get { return _reportInfo.MainException.Message; }
        }

        /// <summary>
        /// Send SMTP email
        /// </summary>
        public void SendSmtp(string exceptionReport, CompletedMethodDelegate setEmailCompletedState)
        {
            var smtpClient = new SmtpClient(_reportInfo.SmtpServer)
                                 {
                                     DeliveryMethod = SmtpDeliveryMethod.Network
                                 };
            MailMessage mailMessage = CreateMailMessage(exceptionReport);

            smtpClient.SendCompleted += delegate { setEmailCompletedState.Invoke(true); };
            smtpClient.SendAsync(mailMessage, null);
        }

        /// <summary>
        /// Send SimpleMAPI email
        /// </summary>
        public void SendMapi(string exceptionReport)
        {
            var mapi = new Mapi();
            string emailAddress = _reportInfo.EmailReportAddress.IsEmpty()
                                      ? _reportInfo.ContactEmail
                                      : _reportInfo.EmailReportAddress;

            mapi.AddRecipient(emailAddress, null, false);
            AddMapiAttachments(mapi);
            mapi.Send(EmailSubject, exceptionReport, true);
        }

        private void AddMapiAttachments(Mapi mapi)
        {
            if (_reportInfo.ScreenshotAvailable)
                mapi.Attach(ScreenshotTaker.GetImageAsFile(_reportInfo.ScreenshotImage));

            foreach (string file in _reportInfo.FilesToAttach)
            {
                mapi.Attach(file);
            }
        }

        private MailMessage CreateMailMessage(string exceptionReport)
        {
            var mailMessage = new MailMessage
                                  {
                                      From = new MailAddress(_reportInfo.SmtpFromAddress, null),
                                      Body = exceptionReport,
                                      Subject = EmailSubject
                                  };
            mailMessage.ReplyToList.Add(new MailAddress(_reportInfo.SmtpFromAddress, null));

            mailMessage.To.Add(new MailAddress(_reportInfo.ContactEmail));
            AddAnyAttachments(mailMessage);

            return mailMessage;
        }

        private void AddAnyAttachments(MailMessage mailMessage)
        {
            AttachScreenshot(mailMessage);
            AttachFiles(mailMessage);
        }

        private void AttachFiles(MailMessage mailMessage)
        {
            foreach (string f in _reportInfo.FilesToAttach)
            {
                mailMessage.Attachments.Add(new Attachment(f));
            }
        }

        private void AttachScreenshot(MailMessage mailMessage)
        {
            if (_reportInfo.ScreenshotAvailable)
                mailMessage.Attachments.Add(new Attachment(ScreenshotTaker.GetImageAsFile(_reportInfo.ScreenshotImage),
                                                           ScreenshotTaker.ScreenshotMimeType));
        }
    }
}