using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetUserDisclosure
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

		public int AssetUserDisclosureId
		{
			get;
			set;
		}

		public DateTime DateConfirmed
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public AssetUserDisclosure()
		{
		}
	}
}