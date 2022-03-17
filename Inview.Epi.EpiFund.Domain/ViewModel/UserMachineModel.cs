using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class UserMachineModel
	{
		public int? AssetNumber
		{
			get;
			set;
		}

		[Required]
		public string Code
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

		[Required]
		public string Username
		{
			get;
			set;
		}

		public UserMachineModel()
		{
		}
	}
}