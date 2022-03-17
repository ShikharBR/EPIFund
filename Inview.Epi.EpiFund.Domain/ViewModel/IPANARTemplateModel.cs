using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class IPANARTemplateModel : IPATemplateViewModels
	{
		public string UserCity
		{
			get;
			set;
		}

		public string UserState
		{
			get;
			set;
		}

		public string UserZip
		{
			get;
			set;
		}

		public IPANARTemplateModel()
		{
		}
	}
}