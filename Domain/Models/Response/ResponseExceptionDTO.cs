namespace Domain.Models.Response
{
    public class ResponseExceptionDTO
    {
        public string Message { get; set; }

        public ResponseExceptionDTO(string message)
        {
            Message = message;
        }
    }
}
