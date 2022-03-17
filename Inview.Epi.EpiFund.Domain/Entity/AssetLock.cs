using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetLock
	{
		public Guid AssetId
		{
			get;
			set;
		}

		public int AssetLockId
		{
			get;
			set;
		}

		public DateTime CreationTime
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public AssetLock()
		{
		}
	}
}