using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class ListingAgentSearchResultsModel : BaseSearchResultsModel
	{
		[Display(Name="Asset Number")]
		public int? AssetNumber
		{
			get;
			set;
		}

		[Display(Name="Commission Share Agr")]
		public bool CommissionShareAgr
		{
			get;
			set;
		}

		[Display(Name="Date Of CSA Confirm")]
		public DateTime? DateOfCsaConfirm
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Cell Number")]
		public string ListingAgentCellNumber
		{
			get;
			set;
		}

		[Display(Name="Listing Agent City")]
		public string ListingAgentCity
		{
			get;
			set;
		}

		[Display(Name="Commission Amount: enter as %")]
		public string ListingAgentCommissionAmount
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Company")]
		public string ListingAgentCompany
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Corp Address 1")]
		public string ListingAgentCorpAddress
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Corp Address 2")]
		public string ListingAgentCorpAddress2
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Email")]
		public string ListingAgentEmail
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Fax Number")]
		public string ListingAgentFaxNumber
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Name")]
		public string ListingAgentName
		{
			get;
			set;
		}

		public string ListingAgentNewName
		{
			get;
			set;
		}

		public string ListingAgentPhoneNumber
		{
			get;
			set;
		}

		[Display(Name="Listing Agent State")]
		public string ListingAgentState
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Main Office Number")]
		public string ListingAgentWorkNumber
		{
			get;
			set;
		}

		[Display(Name="Listing Agent Zip")]
		public string ListingAgentZip
		{
			get;
			set;
		}

		public PagedList.IPagedList<NarMemberViewModel> Members
		{
			get;
			set;
		}

		public ListingAgentSearchResultsModel()
		{
		}
	}
}