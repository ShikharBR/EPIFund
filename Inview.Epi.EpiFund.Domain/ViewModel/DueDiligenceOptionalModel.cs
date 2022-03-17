using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class DueDiligenceOptionalModel
	{
		public Inview.Epi.EpiFund.Domain.Enum.ImportanceLevel ImportanceLevel
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public DueDiligenceOptionalModel()
		{
			this.Text = "Optional";
			this.ImportanceLevel = Inview.Epi.EpiFund.Domain.Enum.ImportanceLevel.Optional;
		}
	}
}