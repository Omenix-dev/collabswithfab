using System;
using System.Linq;
using System.Linq.Expressions;
using MedicalRecordsApi.Services.Common.Interfaces;
using MedicalRecordsData.Enum;
using MedicalRecordsRepository.DTO;

namespace MedicalRecordsApi.Services.Common
{
    public class GenericService<T> : IGenericService<T> where T : class, new()
    {
        //minimum pageIndex = 1, minumum pageSize: 1
        public PaginatedList<T> SortPaginateByText(int pageIndex, int pageSize, IQueryable<T> data, Expression<Func<T, string>> expression, Order order)
        {
            if (data == null)
            {
                return new PaginatedList<T>();
            }

            int dataCount = data.Count();

            //optimization. if data fetched is less than page size and the page index is 1, no need to TakeAndSkip, just return the data
            if (dataCount <= pageSize && pageIndex == 1)
            {
                IQueryable<T> totalSortedData = OrderByText(data, order, expression);
                return new PaginatedList<T>()
                {
                    PageCount = 1,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    Data = totalSortedData.AsEnumerable()
                    //we change data to List /AsEnumerable at this point for immediate execution. Without this, there would be an
                    //error when enumrating over the result .https://github.com/dotnet/efcore/issues/20041
                };
            }

            IQueryable<T> sortedData = OrderByText(data, order, expression);

            IQueryable<T> paginatedResult = TakeAndSkip(sortedData, pageSize, pageIndex);

            int pagecount = (int)Math.Ceiling((double)dataCount / pageSize);

            PaginatedList<T> paginatedList = new PaginatedList<T> { Data = paginatedResult, PageCount = pagecount, PageSize = pageSize, PageIndex = pageIndex };

            return paginatedList;
        }

        //minimum pageIndex = 1, minumum pageSize: 1
        public PaginatedList<T> SortPaginateByDate(int pageIndex, int pageSize, IQueryable<T> data, Expression<Func<T, DateTime>> expression, Order order)
        {
            if (data == null)
            {
                return new PaginatedList<T>();
            }

            int dataCount = data.Count();

            //optimization. if data fetched is less than page size and the page index is 1, no need to TakeAndSkip, just return the data
            if (dataCount <= pageSize && pageIndex == 1)
            {
                IQueryable<T> totalSortedData = OrderByDate(data, order, expression);
                return new PaginatedList<T>()
                {
                    PageCount = 1,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    Data = totalSortedData.AsEnumerable()
                    //we change data to List / AsEnumerable at this point for immediate execution. Without this, there would be an
                    //error when enumrating over the result .https://github.com/dotnet/efcore/issues/20041
                };
            }

            IQueryable<T> sortedData = OrderByDate(data, order, expression);

            IQueryable<T> paginatedResult = TakeAndSkip(sortedData, pageSize, pageIndex);

            int pagecount = (int)Math.Ceiling((double)dataCount / pageSize);

            PaginatedList<T> paginatedList = new PaginatedList<T> { Data = paginatedResult, PageCount = pagecount, PageSize = pageSize, PageIndex = pageIndex };

            return paginatedList;
        }

        public IQueryable<T> OrderByText(IQueryable<T> data, Order order, Expression<Func<T, string>> expression)
        {
            if (order == Order.Asc)
            {
                return data.OrderBy(expression);
            }
            else
            {
                return data.OrderByDescending(expression);
            }
        }

        public IQueryable<T> OrderByDate(IQueryable<T> data, Order order, Expression<Func<T, DateTime>> expression)
        {
            if (order == Order.Asc)
            {
                return data.OrderBy(expression);
            }
            else
            {
                return data.OrderByDescending(expression);
            }
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
    }
}