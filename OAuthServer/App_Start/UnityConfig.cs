namespace OAuthServer
{
	using System;
	using Unity;
	using Models.AccountModels;
	using Services;
	using Shared.DataAccess;
	using Shared.DataAccess.Repositories;

	public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();

	          container.RegisterType<IUserRepository, UserRepository>();
	          container.RegisterType<IConfigRepository, ConfigRepository>();
	          container.RegisterType<IAuthCodeGrantRepository, AuthCodeGrantRepository>();
			  container.RegisterType<UserStore, UserStore>();
	          container.RegisterType<CryptographicService, CryptographicService>();

			  return container;
          });
		  
        public static IUnityContainer Container => container.Value;
    }
}