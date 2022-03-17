using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetDocument
	{
		public Guid AssetDocumentId
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

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

		public bool? Viewable
		{
			get;
			set;
		}

		public AssetDocument()
		{
		}
	}
}