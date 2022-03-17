using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class CookieViewModel
	{
		public List<CookieViewModel.Cookie> Cookies
		{
			get;
			set;
		}

		public CookieViewModel()
		{
			this.Cookies = new List<CookieViewModel.Cookie>();
		}

		public class Cookie
		{
			public string Domain
			{
				get;
				set;
			}

			public string Expires
			{
				get;
				set;
			}

			public string HttpOnly
			{
				get;
				set;
			}

			public string Name
			{
				get;
				set;
			}

			public string Path
			{
				get;
				set;
			}

			public string Shareable
			{
				get;
				set;
			}

			public string TicketExpiration
			{
				get;
				set;
			}

			public string TicketExpired
			{
				get;
				set;
			}

			public string TicketIssueDate
			{
				get;
				set;
			}

			public string TicketPath
			{
				get;
				set;
			}

			public string TicketPersistent
			{
				get;
				set;
			}

			public Cookie()
			{
			}
		}
	}
}