namespace DoughnutBank.Commons.ExpirationTimeStamp
{
    public abstract class TimeStampCreator
    {
        public abstract long createTimeStamp(int number);

        protected long ExpirationTimestamp(int secondsUntilExpiration)
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return currentTime + (secondsUntilExpiration * 1000);
        }
    }
}
