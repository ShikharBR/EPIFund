using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetEscrowModel
	{
		[Display(Name="Actual Closing Date")]
		public DateTime? ActualClosingDate
		{
			get;
			set;
		}

		[Display(Name="Description")]
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

		[Display(Name="Asset ID#")]
		public int AssetNumber
		{
			get;
			set;
		}

		[Display(Name="Closing Price")]
		public double? ClosingPrice
		{
			get;
			set;
		}

		[Display(Name="Contract Buyer of Asset")]
		public string ContractBuyer
		{
			get;
			set;
		}

		[Display(Name="Contract Buyer Address")]
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

		[Display(Name="Listing Price")]
		public double ListingPrice
		{
			get;
			set;
		}

		[Display(Name="Principal Contact of Contract Buyer")]
		public string PrincipalContactOfContractBuyer
		{
			get;
			set;
		}

		[Display(Name="Projected Closing Date")]
		public DateTime? ProjectedClosingDate
		{
			get;
			set;
		}

		public AssetEscrowModel()
		{
		}
	}
}