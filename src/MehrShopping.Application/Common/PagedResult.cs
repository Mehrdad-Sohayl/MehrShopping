namespace MehrShopping.Application.Common
{
    public class PagedResult<T> : Result<T> where T : class
    {
        public int Page { get; }
        public int Size { get; }
        public int TotalCount { get; }

        private PagedResult(
            T value,
            bool isSuccess,
            List<ApplicationError> errors,
            int page,
            int size,
            int totalCount) : base(value, isSuccess, errors)
        {
            Page = page;
            Size = size;
            TotalCount = totalCount;
        }

        public static PagedResult<T> Success(T value, int page, int size, int totalCount)
        {
            return new PagedResult<T>(value, true, new List<ApplicationError>(), page, size, totalCount);
        }
    }
}
