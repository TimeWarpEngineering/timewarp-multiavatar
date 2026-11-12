#region Purpose
// Model that splits a three-character part selection (e.g. "07C") into its two-character part type and one-character theme variant.
#endregion

namespace TimeWarp.Multiavatar;

public class AvatarPart
{
    public string PartType { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    
    public AvatarPart(string selection)
    {
        ArgumentNullException.ThrowIfNull(selection);
        
        if (selection.Length == 3)
        {
            PartType = selection.Substring(0, 2);
            Theme = selection.Substring(2, 1);
        }
    }
}