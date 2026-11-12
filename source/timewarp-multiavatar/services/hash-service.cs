#region Purpose
// Derives the deterministic 12-digit numeric hash from an input string and maps two-digit slices of it to part selections.
#endregion

#region Design
// The SHA-256 hex digest is reduced to its decimal digits and the first 12 are used (zero-padded if fewer). Each 2-digit value (0-99) is scaled by 47/100 to 0-47 and split into a part number and theme variant: 0-15 is variant A, 16-31 is B, 32-47 is C.
#endregion

namespace TimeWarp.Multiavatar;

public static partial class HashService
{
    private const string TwoDigitFormat = "D2";
    
    public static string GetSha256Hash(string input)
    {
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(hashBytes);
    }
    
    public static string GetSha256Numbers(string sha256Hash)
    {
        return NonDigitRegex().Replace(sha256Hash, string.Empty);
    }
    
    public static string GenerateHash(string input)
    {
        string hashHex = GetSha256Hash(input);
        string hashNumbers = GetSha256Numbers(hashHex);
        
        // Take first 12 digits
        return hashNumbers.Length >= 12 ? hashNumbers[..12] : hashNumbers.PadRight(12, '0');
    }
    
    public static string GetPartSelection(string digits)
    {
        int value = int.Parse(digits, CultureInfo.InvariantCulture);
        int scaled = (int)Math.Round(47.0 / 100.0 * value);
        
        if (scaled > 31)
        {
            int nr = scaled - 32;
            return nr.ToString(TwoDigitFormat, CultureInfo.InvariantCulture) + "C";
        }
        else if (scaled > 15)
        {
            int nr = scaled - 16;
            return nr.ToString(TwoDigitFormat, CultureInfo.InvariantCulture) + "B";
        }
        else
        {
            return scaled.ToString(TwoDigitFormat, CultureInfo.InvariantCulture) + "A";
        }
    }

    [GeneratedRegex(@"\D")]
    private static partial Regex NonDigitRegex();
}