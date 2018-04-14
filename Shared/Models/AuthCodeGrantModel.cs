namespace Shared.Models
{
	using System;

	public class AuthCodeGrantModel : Entity
	{
		public Guid UserId { get; set; }

		public string RedirectUri { get; set; }

		public string ClientId { get; set; }

		public Scope Scope { get; set; }

		public DateTime ExpirationDateTimeUtc { get; set; } 

		public bool Lock { get; set; }

		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }
	}
}
