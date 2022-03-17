using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ManagePortfoliosModel
	{
		public string AddressLine1
		{
			get;
			set;
		}

		public string APNNumber
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public string AssetNumber
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.ViewModel.AssetType AssetType
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public UserType ControllingUserType
		{
			get;
			set;
		}

		public string PortfolioName
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public int? UserId
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

        public bool IsSearching
        {
            get;
            set;
        }

        public Guid PortfolioId { get; set; }

		public ManagePortfoliosModel()
		{
		}
	}
}