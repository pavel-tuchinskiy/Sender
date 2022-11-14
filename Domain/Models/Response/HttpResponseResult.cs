namespace Domain.Models.Response
{
    public class HttpResponseResult<T> where T : class
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public ResponseExceptionDTO Exception { get; set; }

        public HttpResponseResult(int statusCode, T data = null, ResponseExceptionDTO exception = null)
        {
            StatusCode = statusCode;
            Data = data;
            Exception = exception;
        }
    }
}
