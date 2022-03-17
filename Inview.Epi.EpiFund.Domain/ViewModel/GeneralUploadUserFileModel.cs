using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;
using System.Web;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class GeneralUploadUserFileModel
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

		public HttpPostedFileBase File
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

		public UploadUserFileType Type
		{
			get;
			set;
		}

		public int UserFileId
		{
			get;
			set;
		}

		public GeneralUploadUserFileModel()
		{
		}
	}
}