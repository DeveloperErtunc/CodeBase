namespace CodeBase.Helpers;
public static class DateHelper
{
    public static string GetDateStr(this DateTimeOffset date, string pattern = "dd/MM/yyyy HH:mm")
    {
        var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
        DateTimeOffset localTime = TimeZoneInfo.ConvertTime(date, info);
        var strDate = localTime.ToString(pattern);
        return strDate;
    }
}
