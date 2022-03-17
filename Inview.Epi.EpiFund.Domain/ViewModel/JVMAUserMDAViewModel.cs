using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class JVMAUserMDAViewModel
	{
		public DateTime? ActualCOE
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

		public Inview.Epi.EpiFund.Domain.ViewModel.AssetType AssetType
		{
			get;
			set;
		}

		public string AssetTypeString
		{
			get;
			set;
		}

		public DateTime? DateOfDFSubmittal
		{
			get;
			set;
		}

		public DateTime? DateOfLOISubmittal
		{
			get;
			set;
		}

		public DateTime? DateOfMDA
		{
			get;
			set;
		}

		public DateTime? DateRefFeePaid
		{
			get;
			set;
		}

		public DateTime? ProposedCOE
		{
			get;
			set;
		}

		public string RefFeePaid
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public IEnumerable<UserRecord> UserRecords
		{
			get;
			set;
		}

		public JVMAUserMDAViewModel()
		{
		}
	}
}