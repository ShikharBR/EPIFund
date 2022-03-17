using System;
using System.Runtime.CompilerServices;
using System.Web;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ImportPrincipalInvestorModel
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

		public bool CheckAgainstNARs
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

		public ImportPrincipalInvestorModel()
		{
			this.CheckAgainstMBAs = true;
			this.CheckAgainstNARs = true;
			this.ShowCheckbox = false;
			this.AreReferredUsers = true;
		}
	}
}