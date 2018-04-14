namespace OAuthServer.Models.AccountModels
{
	using System;
	using Microsoft.AspNet.Identity;
	using Shared.Models;

	public class ApplicationUserManager : UserManager<User, Guid>
	{
		public ApplicationUserManager(IUserStore<User, Guid> store)
			: base(store)
		{
			UserValidator = new UserValidator<User, Guid>(this)
			{
				AllowOnlyAlphanumericUserNames = false
			};
		}

		public override bool SupportsUserLockout => false;
	}
}