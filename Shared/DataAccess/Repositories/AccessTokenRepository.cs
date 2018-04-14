namespace Shared.DataAccess.Repositories
{
    using System;
    using System.Linq;
	using Models;

	public class AuthCodeGrantRepository : DataRepository<AuthCodeGrantModel>, IAuthCodeGrantRepository
	{
		public AuthCodeGrantRepository(DataContext dataContext) : base(dataContext)
		{
		}

		public AuthCodeGrantModel GetByRefreshToken(string refreshToken)
		{
			return Entities.FirstOrDefault(x => x.RefreshToken == refreshToken);
		}

		public AuthCodeGrantModel GetByAccessToken(string accessToken)
		{
			return Entities.FirstOrDefault(x => x.AccessToken == accessToken && x.ExpirationDateTimeUtc > DateTime.UtcNow);
		}
	}
}
