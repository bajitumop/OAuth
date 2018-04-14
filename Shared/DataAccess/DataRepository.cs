namespace Shared.DataAccess
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using System.Threading.Tasks;
	using Models;
	
	public abstract class DataRepository<T> : IDataRepository<T> where T: Entity
	{
		private readonly DataContext _dataContext;

		protected DataRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		protected IQueryable<T> Entities => _dataContext.Set<T>();

		public T Read(Guid id)
		{
			return _dataContext.Set<T>().Find(id);
		}

		public void Create(T entity)
		{
			_dataContext.Entry(entity).State = EntityState.Added;
		}

		public void Update(T entity)
		{
			_dataContext.Entry(entity).State = EntityState.Modified;
		}

		public void Delete(T entity)
		{
			_dataContext.Entry(entity).State = EntityState.Deleted;
		}

		public void Delete(Guid id)
		{
			_dataContext.Entry(new Entity { Id = id }).State = EntityState.Detached;
		}

		public void SaveChanges()
		{
			_dataContext.SaveChanges();
		}

		public async Task<T> ReadAsync(Guid id)
		{
			return await _dataContext.Set<T>().FindAsync(id);
		}

		public Task CreateAsync(T entity)
		{
			Create(entity);
			return Task.FromResult(0);
		}

		public Task UpdateAsync(T entity)
		{
			Update(entity);
			return Task.FromResult(0);
		}

		public Task DeleteAsync(T entity)
		{
			Delete(entity);
			return Task.FromResult(0);
		}

		public Task DeleteAsync(Guid id)
		{
			Delete(id);
			return Task.FromResult(0);
		}

		public async Task SaveChangesAsync()
		{
			await _dataContext.SaveChangesAsync();
		}

		public void Dispose()
		{
			_dataContext?.Dispose();
		}
	}
}
