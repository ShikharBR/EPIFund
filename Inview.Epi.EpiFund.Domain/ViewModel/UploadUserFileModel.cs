using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UploadUserFileModel
	{
		public byte[] File
		{
			get;
			set;
		}

		[Display(Name="File Name")]
		[Required]
		public string FileTitle
		{
			get;
			set;
		}

		[Display(Name="File Upload")]
		[FileExtensions(ErrorMessage="Must choose .pdf file.", Extensions="pdf")]
		[Required]
		public HttpPostedFileBase UploadedDocument
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public UploadUserFileModel()
		{
		}
	}
}