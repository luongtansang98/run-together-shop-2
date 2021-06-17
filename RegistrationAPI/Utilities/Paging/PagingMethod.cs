using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Utilities.Paging
{
	public static class PagingMethod
	{
        public static PagedResult<T> GetPaged<T>(this IQueryable<T> query,
                                         int page, int pageSize = 10, bool isClientSide =false) where T : class
        {
            if (page < 1)
                page = 1;
            if (isClientSide)
                pageSize = 9;
            var result = new PagedResult<T>();
            result.CurrentPage = page; //trang hiện tại
            result.PageSize = pageSize; //tổng record trên 1 trang
            result.RowCount = query.Count(); //total record


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
