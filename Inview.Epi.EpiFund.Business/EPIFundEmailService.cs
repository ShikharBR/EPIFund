using Inview.Epi.EpiFund.Domain;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Postal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Business
{
	public class EPIFundEmailService : IEPIFundEmailService
	{
		private string _mandrillApiKey;

		public EPIFundEmailService(string mandrillApiKey)
		{
			this._mandrillApiKey = mandrillApiKey;
		}

		public MailMessage CreateMailMessage(Email email)
		{
			MailMessage mailMessage;
			string fullPath = Path.GetFullPath("Views\\Emails");
			if (!Directory.Exists(fullPath))
			{
				mailMessage = (new EmailService()).CreateMailMessage(email);
			}
			else
			{
				ViewEngineCollection viewEngineCollection = new ViewEngineCollection()
				{
					new FileSystemRazorViewEngine(fullPath)
				};
				mailMessage = (new EmailService(viewEngineCollection, null)).CreateMailMessage(email);
			}
			return mailMessage;
		}

		public MailMessage CreateMailMessage(string senderEmailAddress, string subject, string body, List<string> recipientAddresses, List<string> attachmentFilenames = null)
		{
			MailMessage mailMessage;
			try
			{
				MailMessage mailAddress = new MailMessage();
				foreach (string recipientAddress in recipientAddresses)
				{
					mailAddress.To.Add(recipientAddress);
				}
				mailAddress.Subject = subject;
				mailAddress.From = new MailAddress(senderEmailAddress, "USCREonline");
				mailAddress.Body = body;
				mailAddress.IsBodyHtml = true;
				if (attachmentFilenames != null)
				{
					foreach (string attachmentFilename in attachmentFilenames)
					{
						if (File.Exists(attachmentFilename))
						{
							System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentFilename, "application/octet-stream");
							ContentDisposition contentDisposition = attachment.ContentDisposition;
							contentDisposition.CreationDate = DateTime.Now;
							contentDisposition.ModificationDate = DateTime.Now;
							contentDisposition.ReadDate = DateTime.Now;
							contentDisposition.FileName = Path.GetFileName(attachmentFilename);
							contentDisposition.Size = (new FileInfo(attachmentFilename)).Length;
							contentDisposition.DispositionType = "attachment";
							mailAddress.Attachments.Add(attachment);
						}
					}
				}
				mailMessage = mailAddress;
			}
			catch (Exception exception)
			{
				mailMessage = null;
			}
			return mailMessage;
		}

		public void DeleteScheduled(string mandrillId)
		{
			try
			{
				MandrillApi mandrillApi = new MandrillApi(this._mandrillApiKey, true);
				mandrillApi.CancelScheduledMessage(new CancelScheduledMessageRequest(mandrillId));
			}
			catch (Exception exception)
			{
			}
		}

		public Task<List<EmailResult>> Send(Email email)
		{
			return this.Send(email, null);
		}

		public async Task<List<EmailResult>> Send(MailMessage email)
		{
			MailAddress to = null;
			List<EmailResult> emailResults;
			MandrillApi mandrillApi = new MandrillApi(this._mandrillApiKey, true);
			List<EmailAddress> emailAddresses = new List<EmailAddress>();
			foreach (MailAddress to1 in email.To)
			{
				emailAddresses.Add(new EmailAddress(to1.Address, to1.DisplayName, "to"));
			}
			foreach (MailAddress cC in email.CC)
			{
				emailAddresses.Add(new EmailAddress(cC.Address, cC.DisplayName, "cc"));
			}
			foreach (MailAddress bcc in email.Bcc)
			{
				emailAddresses.Add(new EmailAddress(bcc.Address, bcc.DisplayName, "bcc"));
			}
			EmailMessage emailMessage = new EmailMessage()
			{
				FromEmail = email.From.Address,
				FromName = email.From.DisplayName,
				To = emailAddresses,
				Subject = email.Subject,
				Html = email.Body
			};
			EmailMessage emailMessage1 = emailMessage;
			try
			{
				emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage1));
			}
			catch
			{
				(new SmtpService()).Send(email);
				emailResults = new List<EmailResult>();
			}
			return emailResults;
		}

		public async Task<List<EmailResult>> Send(Email email, DateTime? sendAt)
		{
			return await this.Send(this.CreateMailMessage(email));
		}
	}
}