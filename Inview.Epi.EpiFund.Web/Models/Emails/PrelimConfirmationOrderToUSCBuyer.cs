using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class PrelimConfirmationOrderToUSCBuyer : PrelimConfirmationModel
	{
		public string Registrant
		{
			get;
			set;
		}

		public PrelimConfirmationOrderToUSCBuyer()
		{
		}
	}
}