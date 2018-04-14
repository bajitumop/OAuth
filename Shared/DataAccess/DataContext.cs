namespace Shared.DataAccess
{
	using System.Data.Entity;
	using Models;

	public class DataContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<Config> Config { get; set; }

		public DbSet<AuthCodeGrantModel> AuthCodeGrantModels { get; set; }
	}
}
