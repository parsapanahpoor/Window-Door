namespace Window.Domain.Entities.Account;

public static class MobileNumber
{
    public static string? Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length > 32)
            return null;

        var digits = new string(ToAsciiDigits(input).Where(c => c is not (' ' or '-' or '(' or ')')).ToArray());
        var subscriber = digits switch
        {
            { Length: 11 } when digits.StartsWith("09", StringComparison.Ordinal) => digits[1..],
            { Length: 13 } when digits.StartsWith("+989", StringComparison.Ordinal) => digits[3..],
            { Length: 14 } when digits.StartsWith("00989", StringComparison.Ordinal) => digits[4..],
            { Length: 12 } when digits.StartsWith("989", StringComparison.Ordinal) => digits[2..],
            { Length: 10 } when digits[0] == '9' => digits,
            _ => null
        };

        return subscriber is not null && subscriber.All(char.IsAsciiDigit) ? "0" + subscriber : null;
    }

    public static string ToAsciiDigits(string input)
    {
        return new string(input.Select(c => c switch
        {
            >= '۰' and <= '۹' => (char)('0' + c - '۰'),
            >= '٠' and <= '٩' => (char)('0' + c - '٠'),
            _ => c
        }).ToArray());
    }
}
