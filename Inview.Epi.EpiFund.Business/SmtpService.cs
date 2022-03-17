using Inview.Epi.EpiFund.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;

namespace Inview.Epi.EpiFund.Business
{
	public class SmtpService : ISmtpService
	{
		private string _senderEmailAddress = "uscre@epifund.com";

        private string _smtpHost = ConfigurationManager.AppSettings["SmtpHost"];

        private string _smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];

        private string _smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

		public SmtpService()
		{
		}

		public void Send(MailMessage message)
		{
			SmtpClient smtpClient = new SmtpClient(this._smtpHost, 25)
			{
				Credentials = new NetworkCredential(this._smtpUsername, this._smtpPassword)
			};
			smtpClient.Send(message);
		}

		public bool Send(string subject, string body, List<string> recipientAddresses, List<string> attachmentFilenames = null)
		{
			bool flag;
			try
			{
				MailMessage mailMessage = new MailMessage();
				foreach (string recipientAddress in recipientAddresses)
				{
					mailMessage.To.Add(recipientAddress);
				}
				mailMessage.Subject = subject;
				mailMessage.From = new MailAddress(this._senderEmailAddress, "USCREonline");
				mailMessage.Body = body;
				mailMessage.IsBodyHtml = true;
				if (attachmentFilenames != null)
				{
					foreach (string attachmentFilename in attachmentFilenames)
					{
						Attachment attachment = new Attachment(attachmentFilename, "application/octet-stream");
						ContentDisposition contentDisposition = attachment.ContentDisposition;
						contentDisposition.CreationDate = DateTime.Now;
						contentDisposition.ModificationDate = DateTime.Now;
						contentDisposition.ReadDate = DateTime.Now;
						contentDisposition.FileName = Path.GetFileName(attachmentFilename);
						contentDisposition.Size = (new FileInfo(attachmentFilename)).Length;
						contentDisposition.DispositionType = "attachment";
						mailMessage.Attachments.Add(attachment);
					}
				}
				SmtpClient smtpClient = new SmtpClient(this._smtpHost, 25)
				{
					Credentials = new NetworkCredential(this._smtpUsername, this._smtpPassword)
				};
				smtpClient.Send(mailMessage);
				flag = true;
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}
	}
}