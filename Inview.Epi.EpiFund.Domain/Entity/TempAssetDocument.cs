using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TempAssetDocument
	{
		public string ContentType
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public Guid GuidId
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}

		public string Size
		{
			get;
			set;
		}

		public int TempAssetDocumentId
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public TempAssetDocument()
		{
		}
	}
}