using System;
using System.Runtime.CompilerServices;
using System.Web;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ImportMbaModel
	{
		public bool AreReferredUsers
		{
			get;
			set;
		}

		public bool CheckAgainstNARMembers
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

		public ImportMbaModel()
		{
			this.CheckAgainstPrincipalInvestors = true;
			this.CheckAgainstNARMembers = true;
			this.ShowCheckbox = false;
			this.AreReferredUsers = true;
		}
	}
}