using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class JVMANetUpViewModel
	{
		public List<AssetUserMDATempModel> AssetMDAs
		{
			get;
			set;
		}

		public string AssetTypes
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public DateTime? DateOfDFSubmittal
		{
			get;
			set;
		}

		public DateTime? DateOfLOISubmittal
		{
			get;
			set;
		}

		public DateTime? DateOfUpload
		{
			get;
			set;
		}

		public DateTime? DateRefRegistered
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string LOILink
		{
			get;
			set;
		}

		public string MDALink
		{
			get;
			set;
		}

		public int? MDAs
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.User User
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public IEnumerable<UserRecord> UserRecords
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Entity.UserReferral UserReferral
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.UserType? UserType
		{
			get;
			set;
		}

		public string UserTypeString
		{
			get;
			set;
		}

		public JVMANetUpViewModel()
		{
		}
	}
}