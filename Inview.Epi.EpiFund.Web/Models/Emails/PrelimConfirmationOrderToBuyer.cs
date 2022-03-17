using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
	public class PrelimConfirmationOrderToBuyer : PrelimConfirmationModel
	{
		public string Registrant
		{
			get;
			set;
		}

		public string TitleCompanyAddress
		{
			get;
			set;
		}

		public string TitleCompanyPhone
		{
			get;
			set;
		}

		public PrelimConfirmationOrderToBuyer()
		{
		}
	}
}