using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class PrelimConfirmationDeliverToUSCBuyer : PrelimConfirmationModel
	{
		public string Registrant
		{
			get;
			set;
		}

		public PrelimConfirmationDeliverToUSCBuyer()
		{
		}
	}
}