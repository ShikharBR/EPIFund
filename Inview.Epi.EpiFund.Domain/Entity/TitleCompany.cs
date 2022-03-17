using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class TitleCompany
	{
		public string City
		{
			get;
			set;
		}

		public DateTime CreatedOn
		{
			get;
			set;
		}

		public double? CurrentRate
		{
			get;
			set;
		}

		public string IncludedStates
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string TitleCompAddress
		{
			get;
			set;
		}

		public string TitleCompAddress2
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public List<TitleCompanyUser> TitleCompanyUsers
		{
			get;
			set;
		}

		public string TitleCompName
		{
			get;
			set;
		}

		public string TitleCompPhone
		{
			get;
			set;
		}

		public string TitleCompURL
		{
			get;
			set;
		}

		public string Zip
		{
			get;
			set;
		}

		public TitleCompany()
		{
		}
	}
}