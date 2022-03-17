using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class PrelimConfirmationDeliverToPISeller : PrelimConfirmationModel
	{
		public string Registrant
		{
			get;
			set;
		}

		public PrelimConfirmationDeliverToPISeller()
		{
		}
	}
}