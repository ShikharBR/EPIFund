using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class UserFile
	{
		public Guid AssetId
		{
			get;
			set;
		}

		public DateTime DateUploaded
		{
			get;
			set;
		}

		public string FileLocation
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public UploadUserFileType? Type
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public int UserFileId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public UserFile()
		{
		}
	}
}