namespace Shared.DataAccess.Repositories
{
	using System.Linq;
	using Models;

	public class ConfigRepository : DataRepository<Config>, IConfigRepository
	{
		public ConfigRepository(DataContext dataContext) : base(dataContext)
		{
		}

		public Config GetConfig()
		{
			return Entities.FirstOrDefault();
		}
	}
}
