using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class JVMAUserMDAViewModels
	{
		public List<JVMAUserMDAViewModel> Assets
		{
			get;
			set;
		}

		public string ParticipantFullName
		{
			get;
			set;
		}

		public string ReferredUserFullName
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public JVMAUserMDAViewModels()
		{
			this.Assets = new List<JVMAUserMDAViewModel>();
		}
	}
}