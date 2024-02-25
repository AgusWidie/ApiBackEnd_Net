namespace APIClinic.Helper
{
    public static class ResponseHelper<T>
    {
        public static GlobalObjectListResponse<T> Create(string message, long Page, long MaxPage, IEnumerable<T> data)
        {
            return new()
            {
                Code = StatusCodes.Status200OK,
                Error = false,
                Message = message,
                Page = Page,
                MaxPage = MaxPage,
                Data = data
            };
        }

        public static GlobalObjectListResponse<T> CreateData(string message, IEnumerable<T> data)
        {
            return new()
            {
                Code = StatusCodes.Status200OK,
                Error = false,
                Message = message,
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
