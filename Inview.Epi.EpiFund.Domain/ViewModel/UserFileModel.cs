using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UserFileModel
	{
		public string DateUploaded
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Location
		{
			get;
			set;
		}

		public UploadUserFileType? Type
		{
			get;
			set;
		}

		public int UserFileId
		{
			get;
			set;
		}

		public UserFileModel()
		{
		}
	}
}