namespace Shared.DataAccess
{
	using System;
	using System.Threading.Tasks;
	using Models;

	public interface IDataRepository<T> : IDisposable
		where T: Entity
	{
		T Read(Guid id);
		void Create(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Delete(Guid id);
		void SaveChanges();

		Task<T> ReadAsync(Guid id);
		Task CreateAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
		Task DeleteAsync(Guid id);
		Task SaveChangesAsync();
	}
}
