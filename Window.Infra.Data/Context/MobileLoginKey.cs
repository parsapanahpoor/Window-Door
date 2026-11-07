namespace Window.Data.Context;

public static class MobileLoginKey
{
    public const string Name = "MobileLoginKey";
    public const string Excluded = "SmsLoginExcluded";
    public const string EligibleFilter = "[SmsLoginExcluded] = 0";

    // Keep this expression fixed for migration 20261107120000. It mirrors MobileNumber.Normalize.
    private const string Compact = "TRANSLATE(REPLACE(REPLACE(REPLACE(REPLACE([Mobile], N' ', N''), N'-', N''), N'(', N''), N')', N''), N'۰۱۲۳۴۵۶۷۸۹٠١٢٣٤٥٦٧٨٩', N'01234567890123456789')";
    private const string Subscriber = $"RIGHT({Compact}, 10)";
    private const string Digits = $"{Subscriber} COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[^0-9]%'";
    private const string Local = $"(LEN({Compact}) = 11 AND {Compact} LIKE N'09%')";
    private const string International = $"((LEN({Compact}) = 13 AND {Compact} LIKE N'+989%') OR (LEN({Compact}) = 14 AND {Compact} LIKE N'00989%') OR (LEN({Compact}) = 12 AND {Compact} LIKE N'989%'))";
    private const string Short = $"(LEN({Compact}) = 10 AND {Compact} LIKE N'9%')";

    public const string ComputedSql = $"CONVERT(nvarchar(32), CASE WHEN {Digits} AND ({Local} OR {International} OR {Short}) THEN N'0' + {Subscriber} ELSE N'legacy:' + CONVERT(nvarchar(20), [Id]) END)";
}
