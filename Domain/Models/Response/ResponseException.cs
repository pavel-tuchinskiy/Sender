namespace Domain.Models.Response
{
    public class ResponseException : Exception
    {
        public string Message { get; set; }

        public ResponseException(string message)
        {
            Message = message;
        }
    }
}
