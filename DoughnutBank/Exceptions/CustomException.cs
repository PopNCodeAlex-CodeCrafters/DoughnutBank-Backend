namespace DoughnutBank.Exceptions
{
    public class CustomException : Exception
    {
        public int ErrorCode = 500;
        public CustomException(string message) : base(message)
        { }
        
        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CustomException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public CustomException(string message, int errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

    }
}
