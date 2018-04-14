namespace Shared.Models
{
	using System;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Microsoft.AspNet.Identity;

	public class User : Entity, IUser<Guid>
	{
		public string UserName { get; set; }

		public string Email { get; set; }

		public string PasswordHash { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, Guid> manager)
		{
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			return userIdentity;
		}
	}
}
