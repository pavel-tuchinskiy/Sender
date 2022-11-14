namespace Domain.Models.Response
{
    public class HttpResponseResult
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public ResponseExceptionDTO Exception { get; set; }

        public HttpResponseResult()
        {

        }

        public HttpResponseResult(int statusCode, object data = null, ResponseExceptionDTO exception = null)
        {
            StatusCode = statusCode;
            Data = data;
            Exception = exception;
        }
    }
}
