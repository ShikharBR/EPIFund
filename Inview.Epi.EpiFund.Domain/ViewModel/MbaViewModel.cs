using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class MbaViewModel : BaseModel
	{
		[Display(Name="Company Address Line 1")]
		public string AddressLine1
		{
			get;
			set;
		}

		[Display(Name="Company Address Line 2")]
		public string AddressLine2
		{
			get;
			set;
		}

		[Display(Name="Cell Phone Number")]
		public string CellPhoneNumber
		{
			get;
			set;
		}

		[Display(Name="Company City")]
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

		[Display(Name="Corporate Entity")]
		public string CorpEntity
		{
			get;
			set;
		}

		[Display(Name="Corporate TIN")]
		public string CorpTIN
		{
			get;
			set;
		}

		[Display(Name="Email Address")]
		public string Email
		{
			get;
			set;
		}

		[Display(Name="Fax Number")]
		public string FaxNumber
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

		[Display(Name="Last Name")]
		public string LastName
		{
			get;
			set;
		}

		public int MbaUserId
		{
			get;
			set;
		}

		[Display(Name="Pending Escrows")]
		public int? PendingEscrows
		{
			get;
			set;
		}

		[Display(Name="Referral db")]
		public double? ReferralDB
		{
			get;
			set;
		}

		[Display(Name="Company State")]
		public string State
		{
			get;
			set;
		}

		[Display(Name="Company Website")]
		public string Website
		{
			get;
			set;
		}

		[Display(Name="Main Office Number")]
		public string WorkNumber
		{
			get;
			set;
		}

		[Display(Name="Company Zip")]
		public string Zip
		{
			get;
			set;
		}

		public MbaViewModel()
		{
			this.IsActive = true;
		}
	}
}