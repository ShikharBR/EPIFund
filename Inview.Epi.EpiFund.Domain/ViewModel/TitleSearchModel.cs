using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class TitleSearchModel
	{
		public bool NeedsTitleManager
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string TitleCompName
		{
			get;
			set;
		}

		public string TitleCompURL
		{
			get;
			set;
		}

		public TitleSearchModel()
		{
		}
	}
}