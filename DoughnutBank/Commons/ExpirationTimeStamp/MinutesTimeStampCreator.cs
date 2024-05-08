namespace DoughnutBank.Commons.ExpirationTimeStamp
{
    public class MinutesTimeStampCreator : TimeStampCreator
    {
        public override long createTimeStamp(int minutesUntilExpiration)
        {
            return ExpirationTimestamp(minutesUntilExpiration * 60);
        }
    }
}
