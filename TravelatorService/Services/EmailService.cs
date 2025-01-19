using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Numerics;
using TravelatorService.Interfaces;

namespace TravelatorService.Services
{
    public class EmailService:IEmailService
    {
        private readonly string _apiKey;
        public EmailService(string apiKey)
        {
            _apiKey = apiKey;
        }
        public async System.Threading.Tasks.Task sendVerificationAsync(string toName, string toEmail, string verificationUrl)
        {
            Configuration.Default.AddApiKey("api-key", "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");

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Travlator";
            string SenderEmail = "shivamat0096@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);
            string HtmlContent = $"<html><body><p>Hi {toName} please verify your email for travlator by clicking<a href = '{verificationUrl}' >here</a></p></body></html> ";
            string TextContent = null;
            string Subject = "Verify Your Email";
            //string BccName = "Janice Doe";
            //string BccEmail = "example2@example2.com";
            //SendSmtpEmailBcc BccData = new SendSmtpEmailBcc(BccEmail, BccName);
            //List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>();
            //Bcc.Add(BccData);
            //string CcName = "John Doe";
            //string CcEmail = "example3@example2.com";
            //SendSmtpEmailCc CcData = new SendSmtpEmailCc(CcEmail, CcName);
            //List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
            //Cc.Add(CcData);
            //string ReplyToName = "John Doe";
            //string ReplyToEmail = "replyto@domain.com";
            //SendSmtpEmailReplyTo ReplyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
            //string AttachmentUrl = null;
            //string stringInBase64 = "aGVsbG8gdGhpcyBpcyB0ZXN0";
            //byte[] Content = System.Convert.FromBase64String(stringInBase64);
            //string AttachmentName = "test.txt";
            //SendSmtpEmailAttachment AttachmentContent = new SendSmtpEmailAttachment(AttachmentUrl, Content, AttachmentName);
            //List<SendSmtpEmailAttachment> Attachment = new List<SendSmtpEmailAttachment>();
            //Attachment.Add(AttachmentContent);

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            }
            catch (Exception e)
            {
            }
        }
    }
}
