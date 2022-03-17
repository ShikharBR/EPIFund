using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class OrderSearchModel
	{
		public string AddressLine1
		{
			get;
			set;
		}

		public string APN
		{
			get;
			set;
		}

		public string AssetCounty
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public int? AssetNumber
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public DateTime? DateOrderedEndDate
		{
			get;
			set;
		}

		public DateTime? DateOrderedStartDate
		{
			get;
			set;
		}

		public DateTime? DatePaidEndDate
		{
			get;
			set;
		}

		public DateTime? DatePaidStartDate
		{
			get;
			set;
		}

		public DateTime? DateSubmittedEndDate
		{
			get;
			set;
		}

		public DateTime? DateSubmittedStartDate
		{
			get;
			set;
		}

		public int InsuranceCompanyId
		{
			get;
			set;
		}

		public AssetType? SelectedAssetType
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public OrderSearchModel()
		{
		}
	}
}