namespace Shared.DataAccess
{
	using System.Threading.Tasks;
	using Models;

	public interface IUserRepository : IDataRepository<User>
	{
		User GetByUserName(string userName);
		Task<User> GetByUserNameAsync(string userName);
	}
}
