using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ManageHoldingCompanyModel
	{
		public string HCName { get; set; }
		public bool ISRA { get; set; }
		public string HCEmail { get; set; }
		public string HCFirstName { get; set; }
		public string HCLastName { get; set; }
		public string LinkedInurl { get; set; }
		public string Facebookurl { get; set; }
		public string Instagramurl { get; set; }
		public string Twitterurl { get; set; }

		public string AssetNumber { get; set; }
		public string AssetName { get; set; }
		public string AddressLine1 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string ApnNumber { get; set; }
		public bool IsPaper { get; set; }
		public string County { get; set; }
		public string ListAgentName { get; set; }

		public ManageHoldingCompanyModel()
		{
		}
	}
	public class ManageOperatingCompanyModel
	{
		public string OCName { get; set; }
		public string OCEmail { get; set; }
		public string OCFirstName { get; set; }
		public string OCLastName { get; set; }
		public string LinkedInurl { get; set; }
		public string Facebookurl { get; set; }
		public string Instagramurl { get; set; }
		public string Twitterurl { get; set; }

		public string AssetNumber { get; set; }
		public string AssetName { get; set; }
		public string AddressLine1 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string ApnNumber { get; set; }
		public bool IsPaper { get; set; }
		public string County { get; set; }
		public string ListAgentName { get; set; }

		public ManageOperatingCompanyModel()
		{
		}
	}
}