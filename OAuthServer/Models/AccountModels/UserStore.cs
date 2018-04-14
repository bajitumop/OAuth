namespace OAuthServer.Models.AccountModels
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNet.Identity;
	using Shared.DataAccess;
	using Shared.Models;

	public class UserStore : IUserPasswordStore<User, Guid>, IUserLockoutStore<User, Guid>, IUserTwoFactorStore<User, Guid>
	{
		private readonly IUserRepository _userRepository;

		public UserStore(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public void Dispose()
		{
			_userRepository?.Dispose();
		}

		public async Task CreateAsync(User user)
		{
			await _userRepository.CreateAsync(user);
			await _userRepository.SaveChangesAsync();
		}

		public async Task UpdateAsync(User user)
		{
			await _userRepository.UpdateAsync(user);
			await _userRepository.SaveChangesAsync();
		}

		public async Task DeleteAsync(User user)
		{
			await _userRepository.DeleteAsync(user);
			await _userRepository.SaveChangesAsync();
		}

		public async Task<User> FindByIdAsync(Guid id)
		{
			return await _userRepository.ReadAsync(id);
		}

		public async Task<User> FindByNameAsync(string userName)
		{
			return await _userRepository.GetByUserNameAsync(userName);
		}

		public async Task SetPasswordHashAsync(User user, string passwordHash)
		{
			user.PasswordHash = passwordHash;
			await Task.FromResult(0);
		}

		public async Task<string> GetPasswordHashAsync(User user)
		{
			return await Task.FromResult(user.PasswordHash);
		}

		public async Task<bool> HasPasswordAsync(User user)
		{
			return await Task.FromResult(true);
		}

		#region IUserLockoutStore and IUserTwoFactorStore implementation
		public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
		{
			throw new NotImplementedException();
		}

		public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
		{
			throw new NotImplementedException();
		}

		public Task<int> IncrementAccessFailedCountAsync(User user)
		{
			throw new NotImplementedException();
		}

		public Task ResetAccessFailedCountAsync(User user)
		{
			throw new NotImplementedException();
		}

		public async Task<int> GetAccessFailedCountAsync(User user)
		{
			return await Task.FromResult(0);
		}

		public async Task<bool> GetLockoutEnabledAsync(User user)
		{
			return await Task.FromResult(false);
		}

		public Task SetLockoutEnabledAsync(User user, bool enabled)
		{
			throw new NotImplementedException();
		}

		public Task SetTwoFactorEnabledAsync(User user, bool enabled)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> GetTwoFactorEnabledAsync(User user)
		{
			return await Task.FromResult(false);
		}
		#endregion
	}
}