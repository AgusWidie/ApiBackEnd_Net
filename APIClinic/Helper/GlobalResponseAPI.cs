namespace APIClinic.Helper
{
    public class GlobalResponse
    {
        public int Code { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
    }

    public class GlobalErrorResponse
    {
        public int Code { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; } = "";
        public string TraceId { get; set; } = "";
    }
    public class GlobalObjectResponse
    {
        public int Code { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; } = "";
        public object Data { get; set; } = "";
    }

    public class GlobalObjectListResponse<T>
    {
        public int Code { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; } = "";
        public long? Page { get; set; } = 1;
        public long? MaxPage { get; set; } = 10;
        public IEnumerable<T> Data { get; set; }
    }

    public class GlobalObjectResponse<T>
    {
        public int Code { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; } = "";
        public T Data { get; set; }
    }
}
