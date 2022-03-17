namespace Inview.Epi.EpiFund.Domain
{
	public interface IEPIContextFactory
	{
		IEPIRepository Create();
	}
}