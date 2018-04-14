namespace Shared.DataAccess
{
	using Models;

	public interface IAuthCodeGrantRepository : IDataRepository<AuthCodeGrantModel>
	{
		AuthCodeGrantModel GetByRefreshToken(string refreshToken);

		AuthCodeGrantModel GetByAccessToken(string accessToken);
	}
}
