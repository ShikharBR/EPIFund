using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class TitleUserSearchResultsModel
	{
		public UserType ControllingUserType
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

		[Display(Name="Full Name")]
		public string FullName
		{
			get
			{
				return string.Concat(this.FirstName, " ", this.LastName);
			}
		}

		[Display(Name="Is Active?")]
		public bool IsActive
		{
			get;
			set;
		}

		[Display(Name="Is Manager?")]
		public bool IsManager
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

		[Display(Name="Managing Officer Name")]
		public string ManagingOfficerName
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		[Display(Name="Phone Number")]
		public string PhoneNumber
		{
			get;
			set;
		}

		public bool ShowActiveOnly
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public string TitleCompanyName
		{
			get;
			set;
		}

		public string TitleCompanyURL
		{
			get;
			set;
		}

		public PagedList.IPagedList<TitleUserQuickViewModel> TitleCompanyUsers
		{
			get;
			set;
		}

		public PagedList.IPagedList<TitleUserQuickViewModel> TitleUsers
		{
			get;
			set;
		}

		public TitleUserSearchResultsModel()
		{
		}
	}
}