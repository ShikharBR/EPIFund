using System;
using System.Runtime.CompilerServices;
using System.Web;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ImportNarMemberModel
	{
		public bool AreReferredUsers
		{
			get;
			set;
		}

		public bool CheckAgainstMBAs
		{
			get;
			set;
		}

		public bool CheckAgainstPrincipalInvestors
		{
			get;
			set;
		}

		public HttpPostedFileBase File
		{
			get;
			set;
		}

		public bool ShowCheckbox
		{
			get;
			set;
		}

		public ImportNarMemberModel()
		{
			this.CheckAgainstPrincipalInvestors = true;
			this.CheckAgainstMBAs = true;
			this.ShowCheckbox = false;
			this.AreReferredUsers = true;
		}
	}
}