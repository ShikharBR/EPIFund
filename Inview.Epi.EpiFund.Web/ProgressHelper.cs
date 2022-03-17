using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Inview.Epi.EpiFund.Web
{
	public class ProgressHelper
	{
		private static object syncRoot;

		private static IDictionary<string, int> ProcessStatus
		{
			get;
			set;
		}

		static ProgressHelper()
		{
			ProgressHelper.syncRoot = new object();
		}

		public ProgressHelper()
		{
			if (ProgressHelper.ProcessStatus == null)
			{
				ProgressHelper.ProcessStatus = new Dictionary<string, int>();
			}
		}

		public void Add(string id)
		{
			lock (ProgressHelper.syncRoot)
			{
				ProgressHelper.ProcessStatus.Add(id, 0);
			}
		}

		public int GetStatus(string id)
		{
			int num;
			lock (ProgressHelper.syncRoot)
			{
				num = (ProgressHelper.ProcessStatus.Keys.Count<string>((string x) => x == id) != 1 ? 100 : ProgressHelper.ProcessStatus[id]);
			}
			return num;
		}

		public string ProcessLongRunningAction(string id)
		{
			for (int i = 1; i <= 100; i++)
			{
				Thread.Sleep(100);
				lock (ProgressHelper.syncRoot)
				{
					ProgressHelper.ProcessStatus[id] = i;
				}
			}
			return id;
		}

		public void Remove(string id)
		{
			lock (ProgressHelper.syncRoot)
			{
				ProgressHelper.ProcessStatus.Remove(id);
			}
		}
	}
}