using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetUserAssignment
	{
		public Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public int AssetUserAssignmentId
		{
			get;
			set;
		}

		public decimal ContractFee
		{
			get;
			set;
		}

		public DateTime? DateFeePaid
		{
			get;
			set;
		}

		public string MiscellaneousNotes
		{
			get;
			set;
		}

		public DateTime? ServiceOrderCompleted
		{
			get;
			set;
		}

		public DateTime ServiceOrderDate
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public AssetUserAssignment()
		{
		}
	}
}