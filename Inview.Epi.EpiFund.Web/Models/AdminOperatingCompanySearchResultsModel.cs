using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class AdminOperatingCompanySearchResultsModel
	{
		[Display(Name = "Name of Operating Co")]
		public string OCName { get; set; }

		[Display(Name = "Operating Co e mail")]
		public string OCEmail { get; set; }

		[Display(Name = "First Name of OC Officer")]
		public string OCFirstName { get; set; }

		[Display(Name = "Last Name of OC Officer")]
		public string OCLastName { get; set; }

		[Display(Name = "LinkedIn url")]
		public string LinkedInurl { get; set; }

		[Display(Name = "Facebook url")]
		public string Facebookurl { get; set; }

		[Display(Name = "Instagram url")]
		public string Instagramurl { get; set; }

		[Display(Name = "Twitter url")]
		public string Twitterurl { get; set; }



		[Display(Name = "Asset ID #")]
		public string AssetNumber { get; set; }

		[Display(Name = "Asset Name")]
		public string AssetName { get; set; }

		[Display(Name = "Address Line 1")]
		public string AddressLine1 { get; set; }

		[Display(Name = "City")]
		public string City { get; set; }

		[Display(Name = "State")]
		public string State { get; set; }

		[Display(Name = "Zip")]
		public string ZipCode { get; set; }

		[Display(Name = "APN Number")]
		public string ApnNumber { get; set; }

		[Display(Name = "Is Paper")]
		public bool IsPaper { get; set; }

		[Display(Name = "County")]
		public string County { get; set; }

		[Display(Name = "List Agent")]
		public string ListAgentName { get; set; }

		public List<SelectListItem> States { get; set; }

		public AdminOperatingCompanySearchResultsModel()
		{
			this.States = Common.GetSelectListItemsOfStates(true);
		}

		public PagedList.IPagedList<OperatingCompanyList> OcList { get; set; }

		public int? Page{get;set;}
		public int? RowCount{get;set;}
	}


}