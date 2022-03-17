using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ChangePasswordModel
	{
		[Compare("Password", ErrorMessage="Password and confirm password do not match.")]
		[DataType(DataType.Password)]
		[Display(Name="Confirm Password")]
		[Required]
		public string ConfirmPassword
		{
			get;
			set;
		}

		[Display(Name="Password")]
		[Required]
		public string Password
		{
			get;
			set;
		}

		public ChangePasswordModel()
		{
		}
	}
}