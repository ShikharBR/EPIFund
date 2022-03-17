using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ResetPasswordModel
	{
		[Display(Name="Email")]
		[Required]
		public string Username
		{
			get;
			set;
		}

		public ResetPasswordModel()
		{
		}
	}
}