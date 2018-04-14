namespace Shared.DataAccess
{
	using System;
	using System.Data.Entity;
	using Models;

	public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DataContext>
	{
		protected override void Seed(DataContext context)
		{
			var config = new Config
			{
				CryptoKey = RandomByteArray(16),
				CryptoIV = RandomByteArray(16),
				AccessTokenExpiration = TimeSpan.FromMinutes(5),
				AccessTokenSize = 32,
				RefreshTokenSize = 32
			};

			context.Config.Add(config);
			context.SaveChanges();
		}

		private static byte[] RandomByteArray(int size)
		{
			var bytes = new byte[size];
			new Random().NextBytes(bytes);
			return bytes;
		}
	}
}
