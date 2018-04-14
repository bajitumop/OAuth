namespace Shared.DataAccess
{
	using Models;

	public interface IConfigRepository : IDataRepository<Config>
	{
		Config GetConfig();
	}
}
