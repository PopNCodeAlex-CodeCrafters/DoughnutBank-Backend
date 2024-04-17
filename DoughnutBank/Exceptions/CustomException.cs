namespace DoughnutBank.Exceptions
{
    public class CustomException : Exception
    {
        public int ErrorCode = 500;
        public CustomException(string message) : base(message)
        { }
        public CustomException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
