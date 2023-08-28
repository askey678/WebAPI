namespace WebApi.BAL.Payloads
{

    public class ApiResponse
    {
        public string message { get; set; }

        public int status { get; set; }

        public ApiResponse() { }
        public ApiResponse(string message, int status)
        {
            this.message = message;
            this.status = status;
        }
    }
}