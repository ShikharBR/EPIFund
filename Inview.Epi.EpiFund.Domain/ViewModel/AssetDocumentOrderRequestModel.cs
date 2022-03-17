using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class AssetDocumentOrderRequestModel
	{
		public string Address1
		{
			get;
			set;
		}

		public string Address2
		{
			get;
			set;
		}

		public string APN
		{
			get;
			set;
		}

		public string AssetDescription
		{
			get;
			set;
		}

		public Guid? AssetGuid
		{
			get;
			set;
		}

		public string AssetId
		{
			get;
			set;
		}

		public string AssetSpecs
		{
			get;
			set;
		}

		public string BuyerAddress1
		{
			get;
			set;
		}

		public string BuyerAddress2
		{
			get;
			set;
		}

		public string BuyerCity
		{
			get;
			set;
		}

		public string BuyerContactInfo
		{
			get;
			set;
		}

		public string BuyerEIN
		{
			get;
			set;
		}

		public string BuyerEmail
		{
			get;
			set;
		}

		public string BuyerName
		{
			get;
			set;
		}

		public string BuyerState
		{
			get;
			set;
		}

		public string BuyerZip
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string CompanyAddress
		{
			get;
			set;
		}

		public string CompanyName
		{
			get;
			set;
		}

		public string CompanyPhone
		{
			get;
			set;
		}

		public List<AssetOrderHistoryViewModel> History
		{
			get;
			set;
		}

		public string InsuranceManagerEmail
		{
			get;
			set;
		}

		public bool IsNARAsset
		{
			get;
			set;
		}

		public bool IsSellerAsset
		{
			get;
			set;
		}

		public string ListingAgent
		{
			get;
			set;
		}

		public string ListingAgentEmail
		{
			get;
			set;
		}

		public TitleUserQuickViewModel Manager
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public string NARMemberEmail
		{
			get;
			set;
		}

		public string NARMemberFullName
		{
			get;
			set;
		}

		public string OrderedByEmail
		{
			get;
			set;
		}

		public string OrderedByName
		{
			get;
			set;
		}

		public string Ownership
		{
			get;
			set;
		}

		public string SellerEmail
		{
			get;
			set;
		}

		public string SellerFullName
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public bool Valid
		{
			get;
			set;
		}

		public string VestingEntity
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public AssetDocumentOrderRequestModel()
		{
			this.History = new List<AssetOrderHistoryViewModel>();
		}
	}
}