using Mandrill.Models;
using Postal;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IEPIFundEmailService
	{
		MailMessage CreateMailMessage(Email email);

		MailMessage CreateMailMessage(string senderEmailAddress, string subject, string body, List<string> recipientAddresses, List<string> attachmentFilenames = null);

		void DeleteScheduled(string mandrillId);

		Task<List<EmailResult>> Send(Email email);

		Task<List<EmailResult>> Send(Email email, DateTime? sendAt = null);

		Task<List<EmailResult>> Send(MailMessage email);
	}
}