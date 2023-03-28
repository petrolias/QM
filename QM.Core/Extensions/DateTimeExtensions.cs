namespace QM.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetDefaultFormat(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd}";
        }
    }
}
