namespace Shared.Models
{
	using System;

	public class Config : Entity
	{	
		public byte[] CryptoKey { get; set; }
		public byte[] CryptoIV { get; set; }
		public int AccessTokenSize { get; set; }
		public int RefreshTokenSize { get; set; }
		public TimeSpan AccessTokenExpiration { get; set; }
	}
}
