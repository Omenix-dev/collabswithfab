using MedicalRecordsRepository.DTO;
using MedicalRecordsRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MedicalRecordsData.Enum;
using MedicalRecordsData.DatabaseContext;

namespace MedicalRecordsRepository
{
	//use Abstract so classes can only inherit rather than instantiate
	//or do not use Abstract. this mean, we can define each sub class DI without
	//having to create a new file.

	//NOTE: For methods with isSave flag
	//If you set iSave to false, you must call SaveChangesToDbAsync() in order to save changes
	//if you set isSave to true, then you must no longer call SaveChangesToDbAsync() for that particular operation because changes would
	//already have been written.

	//public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, new()
	public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
	{
		//protected WellaHealthDbContext _db { get; set; } //since it is an Abstract classm so we can set it from the sub class
		//protected ILogger _logger { get; set; }

		//Responses: failed=0, success=1

		//IEnumerable iterates over an in-memory collection while IQueryable does so on the DB
		// call to .ToList to enable instant query against DB

		protected MedicalRecordDbContext Db;
		protected ILogger Logger;

		public GenericRepository(MedicalRecordDbContext db, ILogger<GenericRepository<T>> logger)
		{
			Logger = logger;
			Db = db;
		}

		public IQueryable<T> GetAll()
		{
			return Db.Set<T>();
		}

		public DbSet<T> GetDbSet()
		{
			return Db.Set<T>();
		}

		public IQueryable<T> Query()
		{
			return Db.Set<T>().AsQueryable();
		}

		#region Async Methods

		public async Task<T> GetByIdAsync(int id)
		{
			var entity = await Db.Set<T>().FindAsync(id);

			return entity;
		}

		public async Task<T> GetByGuidAsync(Guid id)
		{
			var entity = await Db.Set<T>().FindAsync(id);

			return entity;
		}

		public async Task<int> CreateAsync(T entity, bool isSave = true)
		{
			if (entity == null)
			{
				Logger.LogError(RepositoryConstants.CreateNullError, typeof(T).Name);
				return 3;
			}

			Db.Set<T>().Add(entity);

			if (isSave)
			{
				return await SaveChangesToDbAsync();
			}

			return 1;
		}

		public async Task<int> UpdateAsync(T entity, bool isSave = true)
		{
			//Check for this in each overriding implementation or services
			//var prev = await GetById(id);

			//if (prev == null)
			//{
			//    return 0;
			//}

			Db.Set<T>().Update(entity);

			if (isSave)
			{
				return await SaveChangesToDbAsync();
			}

			return 1;
		}

		public async Task<int> DeleteAsync(int id, bool isSave = true)
		{
			T entity = await GetByIdAsync(id);
			if (entity == null)
			{
				Logger.LogError(RepositoryConstants.DeleteNullError, typeof(T).Name);
				return 4;
			}

			Db.Set<T>().Remove(entity);

			if (isSave)
			{
				return await SaveChangesToDbAsync();
			}

			return 1;
			//Task.WaitAll();
		}

		public async Task<int> BulkDeleteAsync(IEnumerable<int> entityId, bool isSave = true)
		{
			if (entityId == null || !entityId.Any())
			{
				Logger.LogError(RepositoryConstants.BulkDeleteNullError, typeof(T).Name);
				return 3;
			}

			DbSet<T> table = Db.Set<T>();

			foreach (int id in entityId)
			{
				T entity = await GetByIdAsync(id);
				if (entity != null)
				{
					table.Remove(entity);
				}
			}

			if (isSave)
			{
				return await SaveChangesToDbAsync();
			}

			return 1;
		}

		public async Task<int> BulkCreateAsync(IEnumerable<T> entities, bool isSave = true)
		{
			if (entities == null || !entities.Any())
			{
				Logger.LogError(RepositoryConstants.BulkCreateNullError, typeof(T).Name);
				return 3;
			}

			DbSet<T> table = Db.Set<T>();

			table.AddRange(entities);

			if (isSave)
			{
				return await SaveChangesToDbAsync();
			}

			return 1;
		}

		//calling this once works since we are using just one DbContext
		//TODO: returning 0 should not lead to 500 error. 0 means no entries were added which may be because all entries have been added already
		//fix this after tests have been writing for projects
		public async Task<int> SaveChangesToDbAsync()
		{
			Logger.LogInformation(RepositoryConstants.LoggingStarted);
			int saveResult;

			try
			{
				int tempResult = await Db.SaveChangesAsync(); //give numbers of entries updated in db. in some cases e.g Update, when no data changes, this method returns 0
				if (tempResult == 0)
				{
					Logger.LogInformation(RepositoryConstants.EmptySaveInfo);
				}
				saveResult = 1; //means atleast one entry was made. 1 is InternalCode.Success.
								//saveResult = tempResult > 0 ? 1 : 0; //means atleast one entry was made. 1 is InternalCode.Success
			}
			catch (DbUpdateConcurrencyException ex)
			{
				Logger.LogError(ex, RepositoryConstants.UpdateConcurrencyException);
				saveResult = -1;
				throw;
			}
			catch (DbUpdateException ex)
			{
				Logger.LogError(ex, RepositoryConstants.UpdateException);
				saveResult = -1;
				throw;
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, RepositoryConstants.SaveChangesException);
				saveResult = -1;
				throw;
			}
			return saveResult;
		}

		public async Task<bool> EntityExistsAsync(int id)
		{
			T entityFound = await Db.Set<T>().FindAsync(id);
			if (entityFound == null)
			{
				return false;
			}

			return true;
		}

		#endregion Async Methods

		#region Non Async methods

		public T GetById(int id)
		{
			var entity = Db.Set<T>().Find(id);

			return entity;
		}

		public T GetByGuid(Guid id)
		{
			var entity = Db.Set<T>().Find(id);

			return entity;
		}

		public int Create(T entity, bool isSave = true)
		{
			if (entity == null)
			{
				Logger.LogError(RepositoryConstants.CreateNullError, typeof(T).Name);
				return 3;
			}

			Db.Set<T>().Add(entity);

			if (isSave)
			{
				return SaveChangesToDb();
			}

			return 1;
		}

		public int Update(T entity, bool isSave = true)
		{
			Db.Set<T>().Update(entity);

			if (isSave)
			{
				return SaveChangesToDb();
			}

			return 1;
		}

		public int Delete(int id, bool isSave = true)
		{
			T entity = GetById(id);
			if (entity == null)
			{
				Logger.LogError(RepositoryConstants.DeleteNullError, typeof(T).Name);
				return 4;
			}

			Db.Set<T>().Remove(entity);

			if (isSave)
			{
				return SaveChangesToDb();
			}

			return 1;
			//Task.WaitAll();
		}

		public int BulkDelete(IEnumerable<int> entityId, bool isSave = true)
		{
			if (entityId == null || !entityId.Any())
			{
				Logger.LogError(RepositoryConstants.BulkDeleteNullError, typeof(T).Name);
				return 3;
			}

			DbSet<T> table = Db.Set<T>();

			foreach (int id in entityId)
			{
				T entity = GetById(id);
				if (entity != null)
				{
					table.Remove(entity);
				}
			}

			if (isSave)
			{
				return SaveChangesToDb();
			}

			return 1;
		}

		public int BulkCreate(IEnumerable<T> entities, bool isSave = true)
		{
			if (entities == null || !entities.Any())
			{
				Logger.LogError(RepositoryConstants.BulkCreateNullError, typeof(T).Name);
				return 3;
			}

			DbSet<T> table = Db.Set<T>();

			table.AddRange(entities);

			if (isSave)
			{
				return SaveChangesToDb();
			}

			return 1;
		}

		//calling this once works since we are using just one DbContext
		//TODO: returning 0 should not lead to 500 error. 0 means no entries were added which may be because all entries have been added already
		//fix this after tests have been writing for projects
		public int SaveChangesToDb()
		{
			Logger.LogInformation(RepositoryConstants.LoggingStarted);

			int saveResult;

			try
			{
				int tempResult = Db.SaveChanges(); //give numbers of entries updated in db. in some cases e.g Update, when no data changes, this method returns 0
				if (tempResult == 0)
				{
					Logger.LogInformation(RepositoryConstants.EmptySaveInfo);
				}
				saveResult = 1; //means atleast one entry was made. 1 is InternalCode.Success.
								//saveResult = tempResult > 0 ? 1 : 0; //means atleast one entry was made. 1 is InternalCode.Success
			}
			catch (DbUpdateConcurrencyException ex)
			{
				Logger.LogError(ex, RepositoryConstants.UpdateConcurrencyException);
				saveResult = -1;
				throw;
			}
			catch (DbUpdateException ex)
			{
				Logger.LogError(ex, RepositoryConstants.UpdateException);
				saveResult = -1;
				throw;
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, RepositoryConstants.SaveChangesException);
				saveResult = -1;
				throw;
			}
			return saveResult;
		}

        public IQueryable<T> TakeAndSkip(IQueryable<T> data, int pageSize, int pageIndex)
        {
            int numRowSkipped = pageSize * (pageIndex - 1);

            if (numRowSkipped == 0)
            {
                return data.Take(pageSize);//.AsEnumerable() //.ToListAsync();
            }

            return data.Skip(numRowSkipped).Take(pageSize);//.AsEnumerable() //.ToListAsync();
        }

        public bool EntityExists(int id)
		{
			T entityFound = Db.Set<T>().Find(id);
			if (entityFound == null)
			{
				return false;
			}

			return true;
		}

		#endregion Non Async methods

		//move to a public class
		public async Task<PaginatedList<T>> SortPaginateByTextAsync(int pageIndex, int pageSize, IQueryable<T> data, Expression<Func<T, string>> expression, Order order)
		{
			int numOfRowsSkipped = (pageIndex - 1) * pageSize;

			IQueryable<T> sortedData = data;
			var dataCount = data.Count();

			if (data == null || dataCount == 0)
			{
				return new PaginatedList<T>();
			}

			sortedData = OrderByText(data, order, expression);

			List<T> paginatedResult = await TakeAndSkipAsync(sortedData, pageSize, pageIndex);

			int pagecount = (int)Math.Ceiling((double)dataCount / pageSize);

			PaginatedList<T> paginatedList = new PaginatedList<T> { Data = paginatedResult, PageCount = pagecount, PageSize = pageSize, PageIndex = pageIndex };

			return paginatedList;
		}

		public async Task<PaginatedList<T>> SortPaginateByDateAsync(int pageIndex, int pageSize, IQueryable<T> data, Expression<Func<T, DateTime>> expression, Order order)
		{
			int numOfRowsSkipped = (pageIndex - 1) * pageSize;

			IQueryable<T> sortedData = data;
			var dataCount = data.Count();

			if (data == null || dataCount == 0)
			{
				return new PaginatedList<T>();
			}

			sortedData = OrderByDate(data, order, expression);

			List<T> paginatedResult = await TakeAndSkipAsync(sortedData, pageSize, pageIndex);

			int pagecount = (int)Math.Ceiling((double)dataCount / pageSize);

			PaginatedList<T> paginatedList = new PaginatedList<T> { Data = paginatedResult, PageCount = pagecount, PageSize = pageSize, PageIndex = pageIndex };

			return paginatedList;
		}

		public IQueryable<T> OrderByText(IQueryable<T> data, Order order, Expression<Func<T, string>> expression)
		{
			IQueryable<T> orderedData = data;

			if (order == Order.Asc)
			{
				orderedData = data.OrderBy(expression);
			}
			else
			{
				orderedData = data.OrderByDescending(expression);
			}

			return orderedData;
		}

		public IQueryable<T> OrderByDate(IQueryable<T> data, Order order, Expression<Func<T, DateTime>> expression)
		{
			IQueryable<T> orderedData = data;

			if (order == Order.Asc)
			{
				orderedData = data.OrderBy(expression);
			}
			else
			{
				orderedData = data.OrderByDescending(expression);
			}

			return orderedData;
		}

		public async Task<List<T>> TakeAndSkipAsync(IQueryable<T> data, int pageSize, int pageIndex)
		{
			//List<T> paginatedList = new List<T>();

			//if (data == null || data.Count() <= 0)
			//    return paginatedList;

			//if (pageSize == 0 && pageIndex == 0)
			//    return paginatedList;

			int numRowSkipped = pageSize * (pageIndex - 1);

			List<T> paginated = await data.Skip(numRowSkipped).Take(pageSize).ToListAsync();

			return paginated;
		}


		//Others
		public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
			=> Db.Set<T>().FirstOrDefaultAsync(predicate);

		public async Task<T> Insert(T entity)
		{
			if (entity == null) throw new ArgumentNullException($"{nameof(Insert)} entity cannot be null");
			try
			{
				await Db.Set<T>().AddAsync(entity);
				await Db.SaveChangesAsync();
				Logger.LogInformation($"Successfully Saved {entity}");

				return entity;
			}
			catch (Exception ex)
			{
				Logger.LogError($"{ex}");
				throw new Exception($"{nameof(entity)} could not be saved: {ex}");
			}
		}

		public async Task Update(T entity)
		{
			if (entity == null) throw new ArgumentNullException($"{nameof(Update)} entity cannot be null");
			// In case AsNoTracking is used
			try
			{
				//Context.Attach(entity);
				//Context.Entry(entity).State = EntityState.Modified;
				//Context.Set<T>().Update(entity);

				Db.Update(entity);
				await Db.SaveChangesAsync();

				Logger.LogInformation($"Successfully Updated {entity}");
			}
			catch (Exception ex)
			{
				Logger.LogError($"{ex}");
			}
		}
	}

}
