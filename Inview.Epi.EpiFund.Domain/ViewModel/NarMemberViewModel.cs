using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class NarMemberViewModel : BaseModel
	{
        //public Inview.Epi.EpiFund.Domain.Enum.AssetType AssetType { get; set; }
        public string AssetType { get; set; }
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

        public bool IsPublished { get; set; }

        public Guid? AssetId
		{
			get;
			set;
		}

		public Guid? AssetNARMemberId
		{
			get;
			set;
		}

		public string AssetNumbers
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

		[Display(Name="Commission Amount: enter as %")]
		public double? CommissionAmount
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

		[Display(Name="Company Name")]
		public string CompanyName
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

		public int NarMemberId
		{
			get;
			set;
		}

		public int? ReferredByUserId
		{
			get;
			set;
		}

		public bool? Registered
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

		public NarMemberViewModel()
		{
			this.IsActive = true;
		}
        public int TotalAssests { get; set; }
		public int TotalPublished { get; set; }
		public int TotalAssetType_MF { get; set; }
		public int TotalAssetType_Office { get; set; }
		public int TotalAssetType_MHP { get; set; }
		public int TotalAssetType_Retail { get; set; }
		public int TotalAssetType_Indus { get; set; }
		public int TotalAssetType_FuelS { get; set; }
		public int TotalAssetType_Med { get; set; }
		


	}
}