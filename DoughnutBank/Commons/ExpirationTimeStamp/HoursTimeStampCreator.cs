namespace DoughnutBank.Commons.ExpirationTimeStamp
{
    public class HoursTimeStampCreator : TimeStampCreator
    {
        public override long createTimeStamp(int hoursUntilExpiration)
        {
            return ExpirationTimestamp(hoursUntilExpiration * 60 * 60);
        }
    }
}
