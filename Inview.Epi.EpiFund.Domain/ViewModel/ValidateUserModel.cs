using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ValidateUserModel
	{
		public bool DoNotShowLinks
		{
			get;
			set;
		}

		[Required]
		public string Email
		{
			get;
			set;
		}

		public int InsuranceCompanyUserId
		{
			get;
			set;
		}

		public string NewICAdmin
		{
			get;
			set;
		}

		[DataType(DataType.Password)]
		[Display(Name="Password")]
		[Required]
		public string Password
		{
			get;
			set;
		}

		public string ReturnUrl
		{
			get;
			set;
		}

		public int TileCompanyUserId
		{
			get;
			set;
		}

		public ValidateUserModel()
		{
		}
	}
}