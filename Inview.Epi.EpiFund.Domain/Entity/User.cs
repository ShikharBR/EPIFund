using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class User
	{
		public virtual string AcronymForCorporateEntity
		{
			get;
			set;
		}

		public virtual string AddressLine1
		{
			get;
			set;
		}

		public virtual string AddressLine2
		{
			get;
			set;
		}

		public virtual string AlternateEmail
		{
			get;
			set;
		}

		public virtual string CellNumber
		{
			get;
			set;
		}

		public virtual string City
		{
			get;
			set;
		}

		public virtual bool CommercialOfficeInterest
		{
			get;
			set;
		}

		public virtual bool CommercialOtherInterest
		{
			get;
			set;
		}

		public virtual bool CommercialRetailInterest
		{
			get;
			set;
		}

		public virtual string CompanyName
		{
			get;
			set;
		}

		public virtual int? CorpAdminId
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Enum.CorporateEntityType CorporateEntityType
		{
			get;
			set;
		}

		public virtual string CorporateTIN
		{
			get;
			set;
		}

		public virtual string CorporateTitle
		{
			get;
			set;
		}

		public virtual string EIN
		{
			get;
			set;
		}

		public virtual string FaxNumber
		{
			get;
			set;
		}

		public virtual string FirstName
		{
			get;
			set;
		}

		public virtual bool FracturedCondoPortfoliosInterest
		{
			get;
			set;
		}

		public virtual bool FuelServicePropertyInterest
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

		public virtual bool GorvernmentTenantPropertyInterest
		{
			get;
			set;
		}

		public virtual bool HasSellerPrivilege
		{
			get;
			set;
		}

		public virtual string HomeNumber
		{
			get;
			set;
		}

		public virtual string ICFileLocation
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Enum.ICStatus? ICStatus
		{
			get;
			set;
		}

		public virtual bool IndustryTenantPropertyInterest
		{
			get;
			set;
		}

		public virtual bool IsActive
		{
			get;
			set;
		}

		public virtual bool IsCertificateOfGoodStandingAvailable
		{
			get;
			set;
		}

		public virtual bool? IsRejectedICAdmin
		{
			get;
			set;
		}

		public virtual string JVMarketerAgreementLocation
		{
			get;
			set;
		}

		public virtual string LastName
		{
			get;
			set;
		}

		public virtual string LicenseDesc
		{
			get;
			set;
		}

		public virtual string LicenseNumber
		{
			get;
			set;
		}

		public virtual string LicenseStateIsHeld
		{
			get;
			set;
		}

		public virtual string ManagingOfficerName
		{
			get;
			set;
		}

		public virtual MBAUser MbaUser
		{
			get;
			set;
		}

		public virtual int? MbaUserId
		{
			get;
			set;
		}

		public virtual bool MedicalTenantPropertyInterest
		{
			get;
			set;
		}

		public virtual bool MHPInterest
		{
			get;
			set;
		}

		public virtual bool MiniStoragePropertyInterest
		{
			get;
			set;
		}

		public virtual bool MixedUseCommercialPropertyInterest
		{
			get;
			set;
		}

		public virtual bool MultiFamilyInterest
		{
			get;
			set;
		}

		public virtual string NCNDFileLocation
		{
			get;
			set;
		}

		public virtual DateTime? NCNDSignDate
		{
			get;
			set;
		}

		public virtual bool NonCompletedDevelopmentsInterest
		{
			get;
			set;
		}

		public virtual bool OfficeTenantPropertyInterest
		{
			get;
			set;
		}

		public virtual bool OptOutAutoEmails
		{
			get;
			set;
		}

		public virtual bool ParkingGaragePropertyInterest
		{
			get;
			set;
		}

		public virtual byte[] Password
		{
			get;
			set;
		}

		public virtual string PreferredContactTimes
		{
			get;
			set;
		}

		public virtual string PreferredMethods
		{
			get;
			set;
		}

		public virtual UserRegisteredReferralStatus ReferralStatus
		{
			get;
			set;
		}

		public virtual int? ReferredByUserId
		{
			get;
			set;
		}

		public virtual bool ResortHotelMotelPropertyInterest
		{
			get;
			set;
		}

		public virtual bool RetailTenantPropertyInterest
		{
			get;
			set;
		}

		public virtual bool SecuredPaperInterest
		{
			get;
			set;
		}

		public virtual DateTime? SignupDate
		{
			get;
			set;
		}

		public virtual bool SingleTenantRetailPortfoliosInterest
		{
			get;
			set;
		}

		public virtual string State
		{
			get;
			set;
		}

		public virtual string StateLicenseDesc
		{
			get;
			set;
		}

		public virtual string StateLicenseNumber
		{
			get;
			set;
		}

		public virtual string StateOfOriginCorporateEntity
		{
			get;
			set;
		}

		public virtual List<UserFile> UserFiles
		{
			get;
			set;
		}

		public virtual int UserId
		{
			get;
			set;
		}

		public virtual string Username
		{
			get;
			set;
		}

		public virtual List<UserNote> UserNotes
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Enum.UserType UserType
		{
			get;
			set;
		}

		public virtual string WorkNumber
		{
			get;
			set;
		}

		public virtual string Zip
		{
			get;
			set;
		}

        public string Initials
        {
            get
            {
                System.Text.StringBuilder initials = new System.Text.StringBuilder();
                if(!string.IsNullOrEmpty(FirstName))
                {
                    initials.Append(FirstName[0]);
                }
                if (!string.IsNullOrEmpty(LastName))
                {
                    initials.Append(LastName[0]);
                }
                return initials.ToString();
            }
        }

		public User()
		{
		}
	}
}