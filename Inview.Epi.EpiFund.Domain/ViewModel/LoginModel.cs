using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class LoginModel
	{
		public List<Guid> AssetIds
		{
			get;
			set;
		}

		[Display(Name="Asset ID#")]
		public int? AssetNumber
		{
			get;
			set;
		}

		public int? AssetSearchCriteriaId
		{
			get;
			set;
		}

		public bool isAdmin
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

		[Display(Name="Email")]
		[Required]
		public string Username
		{
			get;
			set;
		}

		public LoginModel()
		{
			this.AssetIds = new List<Guid>();
		}
	}
}