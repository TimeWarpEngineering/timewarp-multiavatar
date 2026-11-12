#region Purpose
// Replaces the colour placeholders (e.g. "#01;") in an SVG part template with the colours of the selected theme variant.
#endregion

#region Design
// Mirrors the original JavaScript: for each placeholder match, in order, only the first occurrence of that matched text in the current string is replaced by the next colour.
#endregion

namespace TimeWarp.Multiavatar;

public static partial class ColorReplacementService
{
    public static string ReplaceColors(string svgString, string[] colors)
    {
        ArgumentException.ThrowIfNullOrEmpty(svgString);
        ArgumentNullException.ThrowIfNull(colors);
        
        // Find all color placeholders in the SVG
        Regex regex = ColorPlaceholderRegex();
        MatchCollection matches = regex.Matches(svgString);
        
        // Replicate JavaScript's behavior: replace first occurrence of each match
        string resultFinal = svgString;
        
        for (int i = 0; i < matches.Count && i < colors.Length; i++)
        {
            string fullMatch = matches[i].Value;  // e.g., "#01;"
            string replacement = colors[i] + ";";  // e.g., "#ff0000;"
            
            global::TimeWarp.Terminal.Terminal.WriteErrorLine($"Replacing first occurrence of {fullMatch} -> {replacement}");
            
            // Find and replace the FIRST occurrence in the current string
            int index = resultFinal.IndexOf(fullMatch, StringComparison.Ordinal);
            if (index >= 0)
            {
                resultFinal = resultFinal.Remove(index, fullMatch.Length).Insert(index, replacement);
            }
        }
        
        return resultFinal;
    }

    [GeneratedRegex(@"#(.*?);")]
    private static partial Regex ColorPlaceholderRegex();
}