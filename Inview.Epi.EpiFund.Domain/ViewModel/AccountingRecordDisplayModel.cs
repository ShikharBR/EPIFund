using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AccountingRecordDisplayModel
	{
		public string AddressLine1
		{
			get;
			set;
		}

		public List<ICAdminAssetHistoryItem> AssetHistoryItems
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public List<DateTime> ContractDates
		{
			get;
			set;
		}

		public List<ContractFeeDetail> ContractFeeDetails
		{
			get;
			set;
		}

		public UserType ControllingUserType
		{
			get;
			set;
		}

		public DateTime DateOfCurrentSignedICAgreement
		{
			get;
			set;
		}

		public DateTime DateOfCurrentSignedJVMAAgreement
		{
			get;
			set;
		}

		public List<JVMAMDAHistoryItem> MDAHistoryItems
		{
			get;
			set;
		}

		public List<string> MiscellaneousNotes
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int StartYear
		{
			get;
			set;
		}

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

        public string Tin
		{
			get;
			set;
		}

		public double TotalPaidForFiscalYear
		{
			get;
			set;
		}

		public ContractFeePayoutType Type
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public List<SelectListItem> Years
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public AccountingRecordDisplayModel()
		{
			this.AssetHistoryItems = new List<ICAdminAssetHistoryItem>();
			this.ContractDates = new List<DateTime>();
			this.ContractFeeDetails = new List<ContractFeeDetail>();
			this.Years = new List<SelectListItem>();
			for (int i = 2014; i <= DateTime.Now.Year; i++)
			{
				List<SelectListItem> years = this.Years;
				SelectListItem selectListItem = new SelectListItem()
				{
					Selected = DateTime.Now.Year == i,
					Value = i.ToString(),
					Text = i.ToString()
				};
				years.Add(selectListItem);
			}
			this.StartYear = DateTime.Now.Year;
			this.MiscellaneousNotes = new List<string>();

            this.States = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "",
                    Value = ""
                },
                new SelectListItem()
                {
                    Text = "AL",
                    Value = "AL"
                },
                new SelectListItem()
                {
                    Text = "AK",
                    Value = "AK"
                },
                new SelectListItem()
                {
                    Text = "AZ",
                    Value = "AZ"
                },
                new SelectListItem()
                {
                    Text = "AR",
                    Value = "AR"
                },
                new SelectListItem()
                {
                    Text = "CA",
                    Value = "CA"
                },
                new SelectListItem()
                {
                    Text = "CO",
                    Value = "CO"
                },
                new SelectListItem()
                {
                    Text = "CT",
                    Value = "CT"
                },
                new SelectListItem()
                {
                    Text = "DC",
                    Value = "DC"
                },
                new SelectListItem()
                {
                    Text = "DE",
                    Value = "DE"
                },
                new SelectListItem()
                {
                    Text = "FL",
                    Value = "FL"
                },
                new SelectListItem()
                {
                    Text = "GA",
                    Value = "GA"
                },
                new SelectListItem()
                {
                    Text = "HI",
                    Value = "HI"
                },
                new SelectListItem()
                {
                    Text = "ID",
                    Value = "ID"
                },
                new SelectListItem()
                {
                    Text = "IL",
                    Value = "IL"
                },
                new SelectListItem()
                {
                    Text = "IN",
                    Value = "IN"
                },
                new SelectListItem()
                {
                    Text = "IA",
                    Value = "IA"
                },
                new SelectListItem()
                {
                    Text = "KS",
                    Value = "KS"
                },
                new SelectListItem()
                {
                    Text = "KY",
                    Value = "KY"
                },
                new SelectListItem()
                {
                    Text = "LA",
                    Value = "LA"
                },
                new SelectListItem()
                {
                    Text = "ME",
                    Value = "ME"
                },
                new SelectListItem()
                {
                    Text = "MD",
                    Value = "MD"
                },
                new SelectListItem()
                {
                    Text = "MA",
                    Value = "MA"
                },
                new SelectListItem()
                {
                    Text = "MI",
                    Value = "MI"
                },
                new SelectListItem()
                {
                    Text = "MN",
                    Value = "MN"
                },
                new SelectListItem()
                {
                    Text = "MS",
                    Value = "MS"
                },
                new SelectListItem()
                {
                    Text = "MO",
                    Value = "MO"
                },
                new SelectListItem()
                {
                    Text = "MT",
                    Value = "MT"
                },
                new SelectListItem()
                {
                    Text = "NE",
                    Value = "NE"
                },
                new SelectListItem()
                {
                    Text = "NV",
                    Value = "NV"
                },
                new SelectListItem()
                {
                    Text = "NH",
                    Value = "NH"
                },
                new SelectListItem()
                {
                    Text = "NJ",
                    Value = "NJ"
                },
                new SelectListItem()
                {
                    Text = "NM",
                    Value = "NM"
                },
                new SelectListItem()
                {
                    Text = "NY",
                    Value = "NY"
                },
                new SelectListItem()
                {
                    Text = "NC",
                    Value = "NC"
                },
                new SelectListItem()
                {
                    Text = "ND",
                    Value = "ND"
                },
                new SelectListItem()
                {
                    Text = "OH",
                    Value = "OH"
                },
                new SelectListItem()
                {
                    Text = "OK",
                    Value = "OK"
                },
                new SelectListItem()
                {
                    Text = "OR",
                    Value = "OR"
                },
                new SelectListItem()
                {
                    Text = "PA",
                    Value = "PA"
                },
                new SelectListItem()
                {
                    Text = "RI",
                    Value = "RI"
                },
                new SelectListItem()
                {
                    Text = "SC",
                    Value = "SC"
                },
                new SelectListItem()
                {
                    Text = "SD",
                    Value = "SD"
                },
                new SelectListItem()
                {
                    Text = "TN",
                    Value = "TN"
                },
                new SelectListItem()
                {
                    Text = "TX",
                    Value = "TX"
                },
                new SelectListItem()
                {
                    Text = "UT",
                    Value = "UT"
                },
                new SelectListItem()
                {
                    Text = "VT",
                    Value = "VT"
                },
                new SelectListItem()
                {
                    Text = "VA",
                    Value = "VA"
                },
                new SelectListItem()
                {
                    Text = "WA",
                    Value = "WA"
                },
                new SelectListItem()
                {
                    Text = "WV",
                    Value = "WV"
                },
                new SelectListItem()
                {
                    Text = "WI",
                    Value = "WI"
                },
                new SelectListItem()
                {
                    Text = "WY",
                    Value = "WY"
                }
            };

        }
	}
}