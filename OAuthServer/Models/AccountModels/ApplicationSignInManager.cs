namespace OAuthServer.Models.AccountModels
{
	using System;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using Microsoft.AspNet.Identity.Owin;
	using Microsoft.Owin.Security;
	using Shared.Models;

	public class ApplicationSignInManager : SignInManager<User, Guid>
	{
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{
		}

		public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
		{
			return user.GenerateUserIdentityAsync((ApplicationUserManager) UserManager);
		}
	}
}