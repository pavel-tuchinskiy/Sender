namespace Domain.Models.Response
{
    public class HttpResponseResult
    {
        public int StatusCode { get; set; }
        public dynamic Data { get; set; }
        public ResponseExceptionDTO Exception { get; set; }

        public HttpResponseResult(int statusCode, dynamic data = null, ResponseExceptionDTO exception = null)
        {
            StatusCode = statusCode;
            Data = data;
            Exception = exception;
        }
    }
}
