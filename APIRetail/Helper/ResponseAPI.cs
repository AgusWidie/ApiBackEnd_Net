namespace APIRetail.Helper
{
    public static class ResponseHelper<T>
    {
        public static GlobalObjectListResponse<T> Create(string message, IEnumerable<T> data)
        {
            return new()
            {
                Code = StatusCodes.Status200OK,
                Error = false,
                Message = message,
                Data = data
            };
        }

        public static GlobalObjectListResponse<T> CreatePaging(string message, long TotalPageSize, long page, long PageSize, IEnumerable<T> data)
        {
            return new()
            {
                Code = StatusCodes.Status200OK,
                Error = false,
                Message = message,
                TotalPageSize = TotalPageSize,
                Page = page,
                PageSize = PageSize,
                Data = data
            };
        }

        public static GlobalObjectResponse<T> Create(string message, T data)
        {
            return new()
            {
                Code = StatusCodes.Status200OK,
                Error = false,
                Message = message,
                Data = data
            };
        }
    }

    public static class ResponseHelper
    {

        public static GlobalResponse Create(string message)
        {
            return new()
            {
                Code = StatusCodes.Status200OK,
                Error = false,
                Message = message
            };
        }

        public static GlobalResponse CreateError(int code, string message)
        {
            return new()
            {
                Code = code,
                Error = true,
                Message = message
            };
        }
        public static GlobalErrorResponse CreateError(int code, string message, string traceId = null)
        {
            return new()
            {
                Code = code,
                Error = true,
                Message = message,
                TraceId = traceId
            };
        }
    }
}
