using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MedicalRecordsApi.Services.Common.Interfaces
{
    public interface IGenericService<T> where T : class, new()
    {
        IQueryable<T> OrderByDate(IQueryable<T> data, Order order, Expression<Func<T, DateTime>> expression);

        IQueryable<T> OrderByText(IQueryable<T> data, Order order, Expression<Func<T, string>> expression);

        PaginatedList<T> SortPaginateByDate(int pageIndex, int pageSize, IQueryable<T> data, Expression<Func<T, DateTime>> expression, Order order);

        PaginatedList<T> SortPaginateByText(int pageIndex, int pageSize, IQueryable<T> data, Expression<Func<T, string>> expression, Order order);

        IQueryable<T> TakeAndSkip(IQueryable<T> data, int pageSize, int pageIndex);
    }
}
