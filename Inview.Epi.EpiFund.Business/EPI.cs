using Inview.Epi.EpiFund.Domain;
using System;

namespace Inview.Epi.EpiFund.Business
{
	public class EPI : IEPI
	{
		private IEPIRepository _repository;

		public EPI(IEPIRepository repository)
		{
			if (repository == null)
			{
				throw new ArgumentNullException("repository");
			}
			this._repository = repository;
		}

		public void Initialize()
		{
			this._repository.Initialize();
		}
	}
}