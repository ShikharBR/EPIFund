using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetEscrowModel
	{
		[Display(Name = "Actual Closing Date")]
		public DateTime? ActualClosingDate
		{
			get;
			set;
		}

		[Display(Name = "Description")]
		public string AssetDescription
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		[Display(Name = "Asset ID#")]
		public int AssetNumber
		{
			get;
			set;
		}

		[Display(Name = "Closing Price")]
		public double? ClosingPrice
		{
			get;
			set;
		}

		[Display(Name = "Contract Buyer of Asset")]
		public string ContractBuyer
		{
			get;
			set;
		}

		[Display(Name = "Contract Buyer Address")]
		public string ContractBuyerAddress
		{
			get;
			set;
		}

		public bool IsClosing
		{
			get
			{
				return this.ProjectedClosingDate.HasValue;
			}
		}

		[Display(Name = "Listing Price")]
		public double ListingPrice
		{
			get;
			set;
		}

		[Display(Name = "Principal Contact of Contract Buyer")]
		public string PrincipalContactOfContractBuyer
		{
			get;
			set;
		}

		[Display(Name = "Projected Closing Date")]
		public DateTime? ProjectedClosingDate
		{
			get;
			set;
		}

		//added more info
		[Display(Name = "Asset Type")]
		public string AssetType { get; set; }

		[Display(Name = "Asset Name")]
		public string AssetName { get; set; }

		[Display(Name = "Address 1")]
		public string Address { get; set; }

		[Display(Name = "City")]
		public string City { get; set; }

		[Display(Name = "State")]
		public string State { get; set; }

		[Display(Name = "ZipCode")]
		public string ZipCode { get; set; }


		public Guid? HoldingCompanyId{get;set;}

		[Display(Name = "Holding Co")]
		public string HoldingCompany { get; set; }


		public Guid? OperatingCompanyId { get; set; }

		[Display(Name = "Operating Co")]
		public string OperatingCompany { get; set; }

		[Display(Name = "Asset Published")]
		public bool AssetPublished { get; set; }

		[Display(Name = "Login History")]
		public DateTime? LoginHistory { get; set; }

		[Display(Name = "Published Amount")]
		public string PublishedAmount { get; set; }

		[Display(Name = "Bus Driver")]
		public string BusDriver { get; set; }

		public bool Portfolio { get; set; }


		public AssetEscrowModel()
		{
		}
	}
}