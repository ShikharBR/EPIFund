using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class PrelimConfirmationDeliverToBuyer : PrelimConfirmationModel
	{
		public string Registrant
		{
			get;
			set;
		}

		public PrelimConfirmationDeliverToBuyer()
		{
		}
	}
}