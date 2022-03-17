using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class JVMAMDAHistoryItem
	{
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

		public string AssetType
		{
			get;
			set;
		}

		public DateTime DateMDASigned
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		public JVMAMDAHistoryItem()
		{
		}
	}
}