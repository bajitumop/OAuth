namespace OAuthServer.Services
{
	using System;
	using System.IO;
	using System.Security.Cryptography;
	using Shared.DataAccess;
	using Shared.Models;

	public class CryptographicService : IDisposable
	{
		private readonly Config _config;

		private readonly RijndaelManaged _aes = new RijndaelManaged();

		public CryptographicService(IConfigRepository _configRepository)
		{
			_config = _configRepository.GetConfig();
		}

		public byte[] Encrypt(string plainText)
		{
			_aes.IV = _config.CryptoIV;
			_aes.Key = _config.CryptoKey;
			
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, _aes.CreateEncryptor(_aes.Key, _aes.IV), CryptoStreamMode.Write))
				{
					using (var sw = new StreamWriter(cs))
					{
						sw.Write(plainText);
					}

					return ms.ToArray();
				}
			}
		}

		public string Decrypt(byte[] cipher)
		{
			_aes.IV = _config.CryptoIV;
			_aes.Key = _config.CryptoKey;

			using (var ms = new MemoryStream(cipher))
			{
				using (var cs = new CryptoStream(ms, _aes.CreateDecryptor(_aes.Key, _aes.IV), CryptoStreamMode.Read))
				{
					using (var sr = new StreamReader(cs))
					{
						return sr.ReadToEnd();
					}
				}
			}
		}

		public byte[] GenerateBytes(int size)
		{
			var result = new byte[size];
			new Random().NextBytes(result);
			return result;
		}
		
		public void Dispose()
		{
			_aes?.Dispose();
		}
	}
}