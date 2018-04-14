namespace Shared.DataAccess.Repositories
{
	using System.Data.Entity;
	using System.Linq;
	using System.Threading.Tasks;
	using Models;

	public class UserRepository : DataRepository<User>, IUserRepository
	{
		public UserRepository(DataContext dataContext) : base(dataContext)
		{
		}

		public User GetByUserName(string login)
		{
			return Entities.FirstOrDefault(x => x.UserName == login);
		}

		public async Task<User> GetByUserNameAsync(string userName)
		{
			return await Entities.FirstOrDefaultAsync(x => x.UserName == userName);
		}
	}
}
