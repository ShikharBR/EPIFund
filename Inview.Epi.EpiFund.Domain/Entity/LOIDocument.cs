using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class LOIDocument
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

		[Key]
		public Guid LOIDocumentId
		{
			get;
			set;
		}

		public Guid LOIId
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

		public LOIDocumentType Type
		{
			get;
			set;
		}

		public LOIDocument()
		{
		}
	}
}