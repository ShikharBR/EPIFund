using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class PersonalFinancialStatementTemplateModel
	{
		[Display(Name="Accounts Payable")]
		[Required]
		public string AccountsPayable
		{
			get;
			set;
		}

		[Display(Name="Accounts and Notes Receivable")]
		[Required]
		public string AccountsRecievable
		{
			get;
			set;
		}

		[Display(Name="Total")]
		public string AssetsTotal
		{
			get;
			set;
		}

		[Display(Name="Automobiles - Total Present Value")]
		[Required]
		public string Automobiles
		{
			get;
			set;
		}

		[Display(Name="Name of Business")]
		[Required]
		public string BusinessName
		{
			get;
			set;
		}

		[Display(Name="Business Phone")]
		public string BusinessPhone
		{
			get;
			set;
		}

		[Display(Name="Cash on hand and in Banks")]
		[Required]
		public string CashOnHand
		{
			get;
			set;
		}

		[Display(Name="Registration City")]
		[Required]
		public string City
		{
			get;
			set;
		}

		public string CurrentBalance1
		{
			get;
			set;
		}

		public string CurrentBalance2
		{
			get;
			set;
		}

		public string CurrentBalance3
		{
			get;
			set;
		}

		public string CurrentBalance4
		{
			get;
			set;
		}

		public string CurrentBalance5
		{
			get;
			set;
		}

		[Display(Name="Execution Day")]
		public int Day
		{
			get;
			set;
		}

		[Display(Name="Email")]
		[Required]
		public string Email
		{
			get;
			set;
		}

		[Display(Name="As Endorser or Co-Maker")]
		[Required]
		public string EndorserOrCoMaker
		{
			get;
			set;
		}

		[Display(Name="Provisions for Federal Income Tax")]
		[Required]
		public string FederalIncomeTax
		{
			get;
			set;
		}

		public List<SelectListItem> Frequencies
		{
			get;
			set;
		}

		public string Frequency1
		{
			get;
			set;
		}

		public string Frequency2
		{
			get;
			set;
		}

		public string Frequency3
		{
			get;
			set;
		}

		public string Frequency4
		{
			get;
			set;
		}

		public string Frequency5
		{
			get;
			set;
		}

		[Display(Name="Installment Account (Auto)")]
		[Required]
		public string InstallmentAccountAuto
		{
			get;
			set;
		}

		[Display(Name="Monthly Payments")]
		[Required]
		public string InstallmentAccountAutoMonthlyPayment
		{
			get;
			set;
		}

		[Display(Name="Installment Account (Other)")]
		[Required]
		public string InstallmentAccountOther
		{
			get;
			set;
		}

		[Display(Name="Monthly Payment")]
		[Required]
		public string InstallmentAccountOtherMonthlyPayment
		{
			get;
			set;
		}

		[Display(Name="Legal Claims or Judgements")]
		[Required]
		public string LegalClaimsOrJudgments
		{
			get;
			set;
		}

		[Display(Name="Other Liabilities")]
		[Required]
		public string LiabilitiesOtherLiabilities
		{
			get;
			set;
		}

		[Display(Name="Total")]
		public string LiabilitiesTotal
		{
			get;
			set;
		}

		[Display(Name="Total Liabilities")]
		public string LiabilitiesTotalLiabilities
		{
			get;
			set;
		}

		[Display(Name="Unpaid Taxes")]
		[Required]
		public string LiabilitiesUnpaidTaxes
		{
			get;
			set;
		}

		[Display(Name="Life Insurance-Cash Surrender Value Only")]
		[Required]
		public string LifeInsurance
		{
			get;
			set;
		}

		[Display(Name="Life Insurance Held")]
		[Required]
		public string LifeInsuranceHeld
		{
			get;
			set;
		}

		[Display(Name="Loan on Life Insurance")]
		[Required]
		public string LifeInsuranceLoan
		{
			get;
			set;
		}

		[Display(Name="Execution Month")]
		public string Month
		{
			get;
			set;
		}

		public string NameAndAddressOfNoteholders1
		{
			get;
			set;
		}

		public string NameAndAddressOfNoteholders2
		{
			get;
			set;
		}

		public string NameAndAddressOfNoteholders3
		{
			get;
			set;
		}

		public string NameAndAddressOfNoteholders4
		{
			get;
			set;
		}

		public string NameAndAddressOfNoteholders5
		{
			get;
			set;
		}

		[Display(Name="Net Investment Income")]
		[Required]
		public string NetInvestmentIncome
		{
			get;
			set;
		}

		[Display(Name="Net Worth")]
		public string NetWorth
		{
			get;
			set;
		}

		[Display(Name="Notes Payable to Banks and Others")]
		[Required]
		public string NotesPayableToOthers
		{
			get;
			set;
		}

		public string OriginalBalance1
		{
			get;
			set;
		}

		public string OriginalBalance2
		{
			get;
			set;
		}

		public string OriginalBalance3
		{
			get;
			set;
		}

		public string OriginalBalance4
		{
			get;
			set;
		}

		public string OriginalBalance5
		{
			get;
			set;
		}

		[Display(Name="Other Assets")]
		[Required]
		public string OtherAssets
		{
			get;
			set;
		}

		[Display(Name="Other Income (Describe Below)")]
		[Required]
		public string OtherIncome
		{
			get;
			set;
		}

		[Display(Name="Description of Other Income")]
		public string OtherIncomeDescription1
		{
			get;
			set;
		}

		public string OtherIncomeDescription2
		{
			get;
			set;
		}

		public string OtherIncomeDescription3
		{
			get;
			set;
		}

		[Display(Name="Other Liabilities")]
		[Required]
		public string OtherLiabilities
		{
			get;
			set;
		}

		[Display(Name="Other Personal Property")]
		[Required]
		public string OtherProperty
		{
			get;
			set;
		}

		[Display(Name="Other Personal Property and Other Assets")]
		[Required]
		public string OtherPropertyAssets
		{
			get;
			set;
		}

		[Display(Name="IRA or Other Retirement Account")]
		[Required]
		public string OtherRetirementOrIraAccount
		{
			get;
			set;
		}

		[Display(Name="Other Special Debt")]
		[Required]
		public string OtherSpecialDebt
		{
			get;
			set;
		}

		public string PaymentAmount1
		{
			get;
			set;
		}

		public string PaymentAmount2
		{
			get;
			set;
		}

		public string PaymentAmount3
		{
			get;
			set;
		}

		public string PaymentAmount4
		{
			get;
			set;
		}

		public string PaymentAmount5
		{
			get;
			set;
		}

		public int PersonalFinancialStatementId
		{
			get;
			set;
		}

		public string PropertyAAddress
		{
			get;
			set;
		}

		public string PropertyAAmountOfPaymentRecurring
		{
			get;
			set;
		}

		public string PropertyADatePurchased
		{
			get;
			set;
		}

		public string PropertyAMortgageAccountNumber
		{
			get;
			set;
		}

		public string PropertyAMortgageBalance
		{
			get;
			set;
		}

		public string PropertyAMortgageStatus
		{
			get;
			set;
		}

		public string PropertyANameAndAddressOfMortgageHolder
		{
			get;
			set;
		}

		public string PropertyAOriginalCost
		{
			get;
			set;
		}

		public string PropertyAPresentMarketValue
		{
			get;
			set;
		}

		public string PropertyATypeOfRealEstate
		{
			get;
			set;
		}

		public string PropertyBAddress
		{
			get;
			set;
		}

		public string PropertyBAmountOfPaymentRecurring
		{
			get;
			set;
		}

		public string PropertyBDatePurchased
		{
			get;
			set;
		}

		public string PropertyBMortgageAccountNumber
		{
			get;
			set;
		}

		public string PropertyBMortgageBalance
		{
			get;
			set;
		}

		public string PropertyBMortgageStatus
		{
			get;
			set;
		}

		public string PropertyBNameAndAddressOfMortgageHolder
		{
			get;
			set;
		}

		public string PropertyBOriginalCost
		{
			get;
			set;
		}

		public string PropertyBPresentMarketValue
		{
			get;
			set;
		}

		public string PropertyBTypeOfRealEstate
		{
			get;
			set;
		}

		public string PropertyCAddress
		{
			get;
			set;
		}

		public string PropertyCAmountOfPaymentRecurring
		{
			get;
			set;
		}

		public string PropertyCDatePurchased
		{
			get;
			set;
		}

		public string PropertyCMortgageAccountNumber
		{
			get;
			set;
		}

		public string PropertyCMortgageBalance
		{
			get;
			set;
		}

		public string PropertyCMortgageStatus
		{
			get;
			set;
		}

		public string PropertyCNameAndAddressOfMortgageHolder
		{
			get;
			set;
		}

		public string PropertyCOriginalCost
		{
			get;
			set;
		}

		public string PropertyCPresentMarketValue
		{
			get;
			set;
		}

		public string PropertyCTypeOfRealEstate
		{
			get;
			set;
		}

		[Display(Name="Real Estate")]
		[Required]
		public string RealEstate
		{
			get;
			set;
		}

		[Display(Name="Real Estate Income")]
		[Required]
		public string RealEstateIncome
		{
			get;
			set;
		}

		[Display(Name="Mortgages on Real Estate")]
		[Required]
		public string RealEstateMortgage
		{
			get;
			set;
		}

		public List<SelectListItem> RealEstateOwnedOptions
		{
			get;
			set;
		}

		[Display(Name="Registration Address")]
		public string ResidentialAddress
		{
			get;
			set;
		}

		[Display(Name="Cell Phone")]
		public string ResidentialPhone
		{
			get;
			set;
		}

		[Display(Name="Salary")]
		[Required]
		public string Salary
		{
			get;
			set;
		}

		[Display(Name="Savings Account")]
		[Required]
		public string SavingsAccount
		{
			get;
			set;
		}

		[Display(Name="Social Security Number")]
		[Required]
		public string SocialSecurityNumber
		{
			get;
			set;
		}

		[Display(Name="Registration State")]
		[Required]
		public string State
		{
			get;
			set;
		}

		public List<SelectListItem> States
		{
			get;
			set;
		}

		[Display(Name="Stocks and Bonds")]
		[Required]
		public string StocksAndBonds
		{
			get;
			set;
		}

		public string StocksAndBondsCost1
		{
			get;
			set;
		}

		public string StocksAndBondsCost2
		{
			get;
			set;
		}

		public string StocksAndBondsCost3
		{
			get;
			set;
		}

		public string StocksAndBondsCost4
		{
			get;
			set;
		}

		public string StocksAndBondsDateOfExchange1
		{
			get;
			set;
		}

		public string StocksAndBondsDateOfExchange2
		{
			get;
			set;
		}

		public string StocksAndBondsDateOfExchange3
		{
			get;
			set;
		}

		public string StocksAndBondsDateOfExchange4
		{
			get;
			set;
		}

		public string StocksAndBondsMarketValue1
		{
			get;
			set;
		}

		public string StocksAndBondsMarketValue2
		{
			get;
			set;
		}

		public string StocksAndBondsMarketValue3
		{
			get;
			set;
		}

		public string StocksAndBondsMarketValue4
		{
			get;
			set;
		}

		public string StocksAndBondsNameOfSecurities1
		{
			get;
			set;
		}

		public string StocksAndBondsNameOfSecurities2
		{
			get;
			set;
		}

		public string StocksAndBondsNameOfSecurities3
		{
			get;
			set;
		}

		public string StocksAndBondsNameOfSecurities4
		{
			get;
			set;
		}

		public string StocksAndBondsNumberOfShares1
		{
			get;
			set;
		}

		public string StocksAndBondsNumberOfShares2
		{
			get;
			set;
		}

		public string StocksAndBondsNumberOfShares3
		{
			get;
			set;
		}

		public string StocksAndBondsNumberOfShares4
		{
			get;
			set;
		}

		public string StocksAndBondsTotalValue1
		{
			get;
			set;
		}

		public string StocksAndBondsTotalValue2
		{
			get;
			set;
		}

		public string StocksAndBondsTotalValue3
		{
			get;
			set;
		}

		public string StocksAndBondsTotalValue4
		{
			get;
			set;
		}

		public string TypeOfCollateral1
		{
			get;
			set;
		}

		public string TypeOfCollateral2
		{
			get;
			set;
		}

		public string TypeOfCollateral3
		{
			get;
			set;
		}

		public string TypeOfCollateral4
		{
			get;
			set;
		}

		public string TypeOfCollateral5
		{
			get;
			set;
		}

		[Display(Name="Unpaid Taxes")]
		[Required]
		public string UnpaidTaxes
		{
			get;
			set;
		}

		[Display(Name="First Name of Registrant")]
		[Required]
		public string UserFirstName
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		[Display(Name="Last Name of Registrant")]
		[Required]
		public string UserLastName
		{
			get;
			set;
		}

		[Display(Name="Execution Year")]
		public int Year
		{
			get;
			set;
		}

		[Display(Name="Registration Zip")]
		[Required]
		public string Zip
		{
			get;
			set;
		}

		public PersonalFinancialStatementTemplateModel()
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Current",
				Value = "Current"
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "In Arrears",
				Value = "In Arrears"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "In Default",
				Value = "In Default"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "In Foreclosure",
				Value = "In Foreclosure"
			};
			selectListItems.Add(selectListItem3);
			this.RealEstateOwnedOptions = selectListItems;
			List<SelectListItem> selectListItems1 = new List<SelectListItem>();
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Daily",
				Value = "Daily"
			};
			selectListItems1.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "Weekly",
				Value = "Weekly"
			};
			selectListItems1.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "Bi-Weekly",
				Value = "Bi-Weekly"
			};
			selectListItems1.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "Monthly",
				Value = "Monthly"
			};
			selectListItems1.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "Quarterly",
				Value = "Quarterly"
			};
			selectListItems1.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "Semi-Annual",
				Value = "Semi-Annual"
			};
			selectListItems1.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "Yearly",
				Value = "Yearly"
			};
			selectListItems1.Add(selectListItem10);
			this.Frequencies = selectListItems1;
			List<SelectListItem> selectListItems2 = new List<SelectListItem>();
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "AL",
				Value = "AL"
			};
			selectListItems2.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "AK",
				Value = "AK"
			};
			selectListItems2.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "AZ",
				Value = "AZ"
			};
			selectListItems2.Add(selectListItem13);
			SelectListItem selectListItem14 = new SelectListItem()
			{
				Text = "AR",
				Value = "AR"
			};
			selectListItems2.Add(selectListItem14);
			SelectListItem selectListItem15 = new SelectListItem()
			{
				Text = "CA",
				Value = "CA"
			};
			selectListItems2.Add(selectListItem15);
			SelectListItem selectListItem16 = new SelectListItem()
			{
				Text = "CO",
				Value = "CO"
			};
			selectListItems2.Add(selectListItem16);
			SelectListItem selectListItem17 = new SelectListItem()
			{
				Text = "CT",
				Value = "CT"
			};
			selectListItems2.Add(selectListItem17);
			SelectListItem selectListItem18 = new SelectListItem()
			{
				Text = "DC",
				Value = "DC"
			};
			selectListItems2.Add(selectListItem18);
			SelectListItem selectListItem19 = new SelectListItem()
			{
				Text = "DE",
				Value = "DE"
			};
			selectListItems2.Add(selectListItem19);
			SelectListItem selectListItem20 = new SelectListItem()
			{
				Text = "FL",
				Value = "FL"
			};
			selectListItems2.Add(selectListItem20);
			SelectListItem selectListItem21 = new SelectListItem()
			{
				Text = "GA",
				Value = "GA"
			};
			selectListItems2.Add(selectListItem21);
			SelectListItem selectListItem22 = new SelectListItem()
			{
				Text = "HI",
				Value = "HI"
			};
			selectListItems2.Add(selectListItem22);
			SelectListItem selectListItem23 = new SelectListItem()
			{
				Text = "ID",
				Value = "ID"
			};
			selectListItems2.Add(selectListItem23);
			SelectListItem selectListItem24 = new SelectListItem()
			{
				Text = "IL",
				Value = "IL"
			};
			selectListItems2.Add(selectListItem24);
			SelectListItem selectListItem25 = new SelectListItem()
			{
				Text = "IN",
				Value = "IN"
			};
			selectListItems2.Add(selectListItem25);
			SelectListItem selectListItem26 = new SelectListItem()
			{
				Text = "IA",
				Value = "IA"
			};
			selectListItems2.Add(selectListItem26);
			SelectListItem selectListItem27 = new SelectListItem()
			{
				Text = "KS",
				Value = "KS"
			};
			selectListItems2.Add(selectListItem27);
			SelectListItem selectListItem28 = new SelectListItem()
			{
				Text = "KY",
				Value = "KY"
			};
			selectListItems2.Add(selectListItem28);
			SelectListItem selectListItem29 = new SelectListItem()
			{
				Text = "LA",
				Value = "LA"
			};
			selectListItems2.Add(selectListItem29);
			SelectListItem selectListItem30 = new SelectListItem()
			{
				Text = "ME",
				Value = "ME"
			};
			selectListItems2.Add(selectListItem30);
			SelectListItem selectListItem31 = new SelectListItem()
			{
				Text = "MD",
				Value = "MD"
			};
			selectListItems2.Add(selectListItem31);
			SelectListItem selectListItem32 = new SelectListItem()
			{
				Text = "MA",
				Value = "MA"
			};
			selectListItems2.Add(selectListItem32);
			SelectListItem selectListItem33 = new SelectListItem()
			{
				Text = "MI",
				Value = "MI"
			};
			selectListItems2.Add(selectListItem33);
			SelectListItem selectListItem34 = new SelectListItem()
			{
				Text = "MN",
				Value = "MN"
			};
			selectListItems2.Add(selectListItem34);
			SelectListItem selectListItem35 = new SelectListItem()
			{
				Text = "MS",
				Value = "MS"
			};
			selectListItems2.Add(selectListItem35);
			SelectListItem selectListItem36 = new SelectListItem()
			{
				Text = "MO",
				Value = "MO"
			};
			selectListItems2.Add(selectListItem36);
			SelectListItem selectListItem37 = new SelectListItem()
			{
				Text = "MT",
				Value = "MT"
			};
			selectListItems2.Add(selectListItem37);
			SelectListItem selectListItem38 = new SelectListItem()
			{
				Text = "NE",
				Value = "NE"
			};
			selectListItems2.Add(selectListItem38);
			SelectListItem selectListItem39 = new SelectListItem()
			{
				Text = "NV",
				Value = "NV",
				Selected = true
			};
			selectListItems2.Add(selectListItem39);
			SelectListItem selectListItem40 = new SelectListItem()
			{
				Text = "NH",
				Value = "NH"
			};
			selectListItems2.Add(selectListItem40);
			SelectListItem selectListItem41 = new SelectListItem()
			{
				Text = "NJ",
				Value = "NJ"
			};
			selectListItems2.Add(selectListItem41);
			SelectListItem selectListItem42 = new SelectListItem()
			{
				Text = "NM",
				Value = "NM"
			};
			selectListItems2.Add(selectListItem42);
			SelectListItem selectListItem43 = new SelectListItem()
			{
				Text = "NY",
				Value = "NY"
			};
			selectListItems2.Add(selectListItem43);
			SelectListItem selectListItem44 = new SelectListItem()
			{
				Text = "NC",
				Value = "NC"
			};
			selectListItems2.Add(selectListItem44);
			SelectListItem selectListItem45 = new SelectListItem()
			{
				Text = "ND",
				Value = "ND"
			};
			selectListItems2.Add(selectListItem45);
			SelectListItem selectListItem46 = new SelectListItem()
			{
				Text = "OH",
				Value = "OH"
			};
			selectListItems2.Add(selectListItem46);
			SelectListItem selectListItem47 = new SelectListItem()
			{
				Text = "OK",
				Value = "OK"
			};
			selectListItems2.Add(selectListItem47);
			SelectListItem selectListItem48 = new SelectListItem()
			{
				Text = "OR",
				Value = "OR"
			};
			selectListItems2.Add(selectListItem48);
			SelectListItem selectListItem49 = new SelectListItem()
			{
				Text = "PA",
				Value = "PA"
			};
			selectListItems2.Add(selectListItem49);
			SelectListItem selectListItem50 = new SelectListItem()
			{
				Text = "RI",
				Value = "RI"
			};
			selectListItems2.Add(selectListItem50);
			SelectListItem selectListItem51 = new SelectListItem()
			{
				Text = "SC",
				Value = "SC"
			};
			selectListItems2.Add(selectListItem51);
			SelectListItem selectListItem52 = new SelectListItem()
			{
				Text = "SD",
				Value = "SD"
			};
			selectListItems2.Add(selectListItem52);
			SelectListItem selectListItem53 = new SelectListItem()
			{
				Text = "TN",
				Value = "TN"
			};
			selectListItems2.Add(selectListItem53);
			SelectListItem selectListItem54 = new SelectListItem()
			{
				Text = "TX",
				Value = "TX"
			};
			selectListItems2.Add(selectListItem54);
			SelectListItem selectListItem55 = new SelectListItem()
			{
				Text = "UT",
				Value = "UT"
			};
			selectListItems2.Add(selectListItem55);
			SelectListItem selectListItem56 = new SelectListItem()
			{
				Text = "VT",
				Value = "VT"
			};
			selectListItems2.Add(selectListItem56);
			SelectListItem selectListItem57 = new SelectListItem()
			{
				Text = "VA",
				Value = "VA"
			};
			selectListItems2.Add(selectListItem57);
			SelectListItem selectListItem58 = new SelectListItem()
			{
				Text = "WA",
				Value = "WA"
			};
			selectListItems2.Add(selectListItem58);
			SelectListItem selectListItem59 = new SelectListItem()
			{
				Text = "WV",
				Value = "WV"
			};
			selectListItems2.Add(selectListItem59);
			SelectListItem selectListItem60 = new SelectListItem()
			{
				Text = "WI",
				Value = "WI"
			};
			selectListItems2.Add(selectListItem60);
			SelectListItem selectListItem61 = new SelectListItem()
			{
				Text = "WY",
				Value = "WY"
			};
			selectListItems2.Add(selectListItem61);
			this.States = selectListItems2;
		}
	}
}