namespace OAuthServer.Models
{
	using Shared;

	public class AuthorizationCodeGrantModel
	{
		public string RedirectUri { get; set; }
		
		public string ClientId { get; set; }

		public string ScopeStr { get; set; }
        
		public string State { get; set; }
	}
}