namespace ds_rca.utils;

public static class PriceFormatter
{
    public static string FormatToPrice(this string price)
    {
        decimal dollars = decimal.Parse(price) / 100;
        string formatted = string.Format("${0:0.00}", dollars);
        return formatted;
    }
}