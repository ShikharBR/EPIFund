using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UserQuickViewModel
	{
		public string AddressLine1
		{
			get;
			set;
		}

		public string AssetsOfInterest
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string CompanyName
		{
			get;
			set;
		}

		public string CorpEntity
		{
			get;
			set;
		}

		public string CorpTIN
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

		public bool HasJVMA
		{
			get;
			set;
		}

		public bool HasNCND
		{
			get;
			set;
		}

		public bool HasPOFCLA
		{
			get;
			set;
		}

		public bool HasSearchCriteria
		{
			get;
			set;
		}

		public int ICCount
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.ICStatus? ICStatus
		{
			get;
			set;
		}

		public bool InEscrow
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public bool IsASeller
		{
			get;
			set;
		}

		public int Last30Assets
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string LicenseType
		{
			get;
			set;
		}

		public int NumberOfSearchCriteria
		{
			get;
			set;
		}

		public int NumberOfSignedMDAs
		{
			get;
			set;
		}

		public string OperatingLicenseNumber
		{
			get;
			set;
		}

		public int PendingAssets
		{
			get;
			set;
		}

		public int? PendingEscrows
		{
			get;
			set;
		}

		public int ReferralDB
		{
			get;
			set;
		}

		public bool ReferredByJVMA
		{
			get;
			set;
		}

		public int? ReferredByUserId
		{
			get;
			set;
		}

		public DateTime RegisterDate
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string StateLicenseIsHeld
		{
			get;
			set;
		}

		public string TIN
		{
			get;
			set;
		}

		public int TotalNewAssets
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.UserType UserType
		{
			get;
			set;
		}

		public string UserTypeDescription
		{
			get;
			set;
		}

		public string UserTypeString
		{
			get;
			set;
		}

        public DateTime? SignupDate { get; set; }

		public UserQuickViewModel()
		{
		}
	}
}