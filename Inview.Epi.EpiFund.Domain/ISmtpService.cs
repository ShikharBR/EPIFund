using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Inview.Epi.EpiFund.Domain
{
	public interface ISmtpService
	{
		bool Send(string subject, string body, List<string> recipientAddresses, List<string> attachmentFilenames = null);

		void Send(MailMessage message);
	}
}