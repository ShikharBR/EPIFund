using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class SignedMDAQuickViewModel
	{
		public string AssetAddressLine1
		{
			get;
			set;
		}

		public string AssetCity
		{
			get;
			set;
		}

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

		public int AssetNumber
		{
			get;
			set;
		}

		public string AssetState
		{
			get;
			set;
		}

		public int AssetUserMDAId
		{
			get;
			set;
		}

		public ListingStatus CurrentListingStatus
		{
			get;
			set;
		}

		public DateTime DateSigned
		{
			get;
			set;
		}

		public string PropertyType
		{
			get;
			set;
		}

		public string UnitDescription
		{
			get;
			set;
		}

		public SignedMDAQuickViewModel()
		{
		}
	}
}