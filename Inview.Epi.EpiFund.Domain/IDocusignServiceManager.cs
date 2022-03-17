using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IDocusignServiceManager
	{
		List<DocumentType> ProcessSignedDocuments(int? userId = null);

		void Start(EventLog log);

		void Stop();

		void TestSend();
	}
}