using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class UserSearchResultsModel : BaseSearchResultsModel
	{
		[Display(Name="Address Line 1")]
		public string AddressLine1
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Company Name")]
		public string CompanyName
		{
			get;
			set;
		}

		public UserType ControllingUserType
		{
			get;
			set;
		}

		public int? CorpAdminId
		{
			get;
			set;
		}

		[Display(Name="Corporate Entity")]
		public string CorpEntity
		{
			get;
			set;
		}

		public bool DoesPIHaveSellerPrivilege
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		[Display(Name="First Name")]
		public string FirstName
		{
			get;
			set;
		}

		public bool HasJVMA
		{
			get;
			set;
		}

		public PagedList.IPagedList<PrincipalInvestorQuickViewModel> Investors
		{
			get;
			set;
		}

		public bool IsASeller
		{
			get;
			set;
		}

		[Display(Name="Last Name")]
		public string LastName
		{
			get;
			set;
		}

		public PagedList.IPagedList<MbaViewModel> Members
		{
			get;
			set;
		}

		public int? PendingEscrows
		{
			get;
			set;
		}

		public double? ReferralDB
		{
			get;
			set;
		}

		[Display(Name="Registration Date End")]
		public DateTime? RegistrationDateEnd
		{
			get;
			set;
		}

		[Display(Name="Registration Date Start")]
		public DateTime? RegistrationDateStart
		{
			get;
			set;
		}

		[Display(Name="User Type")]
		public UserType? SelectedUserType
		{
			get;
			set;
		}

		[Display(Name="Show Active Only?")]
		public bool ShowActiveOnly
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string State
		{
			get;
			set;
		}

		public PagedList.IPagedList<UserQuickViewModel> Users
		{
			get;
			set;
		}

		public List<SelectListItem> UserTypes
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public UserSearchResultsModel()
		{
			this.UserTypes = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = null,
					Text = "All",
					Selected = true
				},
				new SelectListItem()
				{
					Value = UserType.CorpAdmin.ToString(),
					Text = "Corp Admin"
				},
				new SelectListItem()
				{
					Value = UserType.CREBroker.ToString(),
					Text = "CRE Broker"
				},
				new SelectListItem()
				{
					Value = UserType.CRELender.ToString(),
					Text = "CRE Lender"
				},
				new SelectListItem()
				{
					Value = UserType.CorpAdmin2.ToString(),
					Text = "Corp Admin 2"
				},
				new SelectListItem()
				{
					Value = UserType.Investor.ToString(),
					Text = "Investor"
				},
				new SelectListItem()
				{
					Value = UserType.ListingAgent.ToString(),
					Text = "Listing Agent"
				},
				new SelectListItem()
				{
					Value = UserType.SiteAdmin.ToString(),
					Text = "Site Admin"
				}
			};
		}
	}
}