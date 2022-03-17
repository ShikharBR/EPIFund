using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class LOI
	{
		public bool Active
		{
			get;
			set;
		}

		public string AssessorNumber
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public double? BalanceEarnestDeposit
		{
			get;
			set;
		}

		public string BeneficiarySeller
		{
			get;
			set;
		}

		public string BusinessPhoneNumber
		{
			get;
			set;
		}

		public string Buyer1
		{
			get;
			set;
		}

		public string Buyer1Name
		{
			get;
			set;
		}

		public string Buyer2
		{
			get;
			set;
		}

		public string BuyerAssigneeName
		{
			get;
			set;
		}

		public string BuyersAssignee1
		{
			get;
			set;
		}

		public string BuyersAssignee1Officer
		{
			get;
			set;
		}

		public string BuyersAssignee1Title
		{
			get;
			set;
		}

		public string BuyersAssignee2
		{
			get;
			set;
		}

		public string BuyersAssignee2Officer
		{
			get;
			set;
		}

		public string BuyersAssignee2Title
		{
			get;
			set;
		}

		public string BuyerTitle1
		{
			get;
			set;
		}

		public string BuyerTitle2
		{
			get;
			set;
		}

		public string CareOf
		{
			get;
			set;
		}

		public string CellPhoneNumber
		{
			get;
			set;
		}

		public string ClosingDate
		{
			get;
			set;
		}

		public string ClosingDateNumberOfDays
		{
			get;
			set;
		}

		public string CommissionFeesName
		{
			get;
			set;
		}

		public string CommissionFeesNumber
		{
			get;
			set;
		}

		public string Company
		{
			get;
			set;
		}

		public string CREAquisitionLOI
		{
			get;
			set;
		}

		public string Date
		{
			get;
			set;
		}

		public string DueDiligenceDate
		{
			get;
			set;
		}

		public string DueDiligenceNumberOfDays
		{
			get;
			set;
		}

		public string EmailAddress
		{
			get;
			set;
		}

		public string EmailAddress2
		{
			get;
			set;
		}

		public string EscrowCompanyAddress
		{
			get;
			set;
		}

		public string EscrowCompanyAddress2
		{
			get;
			set;
		}

		public string EscrowCompanyCity
		{
			get;
			set;
		}

		public string EscrowCompanyName
		{
			get;
			set;
		}

		public string EscrowCompanyPhoneNumber
		{
			get;
			set;
		}

		public string EscrowCompanyState
		{
			get;
			set;
		}

		public string EscrowCompanyZip
		{
			get;
			set;
		}

		public string FaxNumber
		{
			get;
			set;
		}

		public string FormalDocumentationDate
		{
			get;
			set;
		}

		public string FormalDocumentationNumberOfDays
		{
			get;
			set;
		}

		public string From
		{
			get;
			set;
		}

		public double? InitialEarnestDeposit
		{
			get;
			set;
		}

		public string LegalDescription
		{
			get;
			set;
		}

		public string Lender
		{
			get;
			set;
		}

		public DateTime LOIDate
		{
			get;
			set;
		}

		[Key]
		public Guid LOIId
		{
			get;
			set;
		}

		public bool NoSecuredMortgages
		{
			get;
			set;
		}

		public string ObjectOfPurchase
		{
			get;
			set;
		}

		public double OfferingPurchasePrice
		{
			get;
			set;
		}

		public string OfficePhone
		{
			get;
			set;
		}

		public string OfficerOfSeller
		{
			get;
			set;
		}

		public string OperatingDisclosureDate
		{
			get;
			set;
		}

		public string OperatingDisclosureNumberOfDays
		{
			get;
			set;
		}

		public bool ReadByListedByUser
		{
			get;
			set;
		}

		public string Releasing
		{
			get;
			set;
		}

		public string SecuredMortgages
		{
			get;
			set;
		}

		public string SellerDisclosureDate
		{
			get;
			set;
		}

		public string SellerDisclosureNumberOfDays
		{
			get;
			set;
		}

		public string SellerReceiver1
		{
			get;
			set;
		}

		public string SellerReceiver1Officer
		{
			get;
			set;
		}

		public string SellerReceiver1Title
		{
			get;
			set;
		}

		public string SellerReceiver2
		{
			get;
			set;
		}

		public string SellerReceiver2Officer
		{
			get;
			set;
		}

		public string SellerReceiver2Title
		{
			get;
			set;
		}

		public string StateOfCountyAssessors
		{
			get;
			set;
		}

		public string StateOfPropertyTaxOffice
		{
			get;
			set;
		}

		public string Terms1
		{
			get;
			set;
		}

		public string Terms2
		{
			get;
			set;
		}

		public string Terms3
		{
			get;
			set;
		}

		public double? TermsOfPurchase
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public int TotalNumberOfPagesIncludingCover
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string WebsiteEmail
		{
			get;
			set;
		}

		public string WorkPhoneNumber
		{
			get;
			set;
		}

		public LOI()
		{
		}
	}
}