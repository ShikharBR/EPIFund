using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class UserReferralModel
	{
		[Display(Name="User's Email")]
		[Required(ErrorMessage="User Email is required")]
		public string Email
		{
			get;
			set;
		}

		public UserReferralModel()
		{
		}
	}
}