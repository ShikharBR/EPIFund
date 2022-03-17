using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class JVMANetUpSearchModel
	{
		public string AssetType
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public DateTime? DateOfDFSubmittalEnd
		{
			get;
			set;
		}

		public DateTime? DateOfDFSubmittalStart
		{
			get;
			set;
		}

		public DateTime? DateOfLOISubmittalEnd
		{
			get;
			set;
		}

		public DateTime? DateOfLOISubmittalStart
		{
			get;
			set;
		}

		public DateTime? DateOfUploadEnd
		{
			get;
			set;
		}

		public DateTime? DateOfUploadStart
		{
			get;
			set;
		}

		public DateTime? DateRefRegisteredEnd
		{
			get;
			set;
		}

		public DateTime? DateRefRegisteredStart
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

		public string State
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.UserType? UserType
		{
			get;
			set;
		}

		public JVMANetUpSearchModel()
		{
		}
	}
}