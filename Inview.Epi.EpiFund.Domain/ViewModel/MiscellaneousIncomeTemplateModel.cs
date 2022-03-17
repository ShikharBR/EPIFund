using System.ComponentModel.DataAnnotations;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class MiscellaneousIncomeTemplateModel : BaseIncomeTemplateModel
	{
		[Display(Name="Gross Proceeds Paid to an Attorney")]
		public double? AttorneyPayment
		{
			get;
			set;
		}

		[Display(Name="Crop Insurance Proceeds")]
		public double? CropInsuranceProceeds
		{
			get;
			set;
		}

		[Display(Name="Section 409A Deferrals")]
		public double? Deferrals
		{
			get;
			set;
		}

		[Display(Name="Fishing Boat Proceeds")]
		public double? FishingBoatProceeds
		{
			get;
			set;
		}

		[Display(Name="Section 409A Income")]
		public double? Income
		{
			get;
			set;
		}

		[Display(Name="Medical and Health Care Payments")]
		public double? MedicalPayments
		{
			get;
			set;
		}

		public int MiscellaneousIncomeId
		{
			get;
			set;
		}

		[Display(Name="Non-employee Compensation")]
		public double? NonEmployeeCompensation
		{
			get;
			set;
		}

		[Display(Name="Other Income")]
		public double? OtherIncome
		{
			get;
			set;
		}

		[Display(Name="Excess Golden Parachute Payments")]
		public double? ParachutePayments
		{
			get;
			set;
		}

		[Display(Name="Payer made direct sales of $5,000 or more of consumer products to a buyer (recipient) for resale")]
		public bool? PayerExceededSales
		{
			get;
			set;
		}

		[Display(Name="Rents")]
		public double? Rents
		{
			get;
			set;
		}

		[Display(Name="Royalties")]
		public double? Royalties
		{
			get;
			set;
		}

		[Display(Name="2nd TIN not.")]
		public bool? SecondTIN
		{
			get;
			set;
		}

		[Display(Name="State Income 1")]
		public double? StateIncome1
		{
			get;
			set;
		}

		[Display(Name="State Income 2")]
		public double? StateIncome2
		{
			get;
			set;
		}

		[Display(Name="State/Payer's State Number 1")]
		public string StateNumber1
		{
			get;
			set;
		}

		[Display(Name="State/Payer's State Number 2")]
		public string StateNumber2
		{
			get;
			set;
		}

		[Display(Name="Substitute Payments in Lieu of Dividends or Interest")]
		public double? SubstitutePayments
		{
			get;
			set;
		}

		[Display(Name="Federal Income Tax Withheld")]
		public double? TaxWithheld
		{
			get;
			set;
		}

		[Display(Name="State Tax Withheld 1")]
		public double? TaxWithheld1
		{
			get;
			set;
		}

		[Display(Name="State Tax Withheld 2")]
		public double? TaxWithheld2
		{
			get;
			set;
		}

        [Display(Name ="FACTA Filing Requirement")]
        public bool FactaFilingRequirement
        {
            get;
            set;
        }

        public int UserId
		{
			get;
			set;
		}

		public MiscellaneousIncomeTemplateModel()
		{
		}
	}
}