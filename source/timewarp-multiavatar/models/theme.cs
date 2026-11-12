#region Purpose
// Model for one theme JSON resource: colour arrays per part name for each of the A, B and C theme variants.
#endregion

namespace TimeWarp.Multiavatar;

public class Theme
{
    public Dictionary<string, string[]> A { get; init; } = [];
    public Dictionary<string, string[]> B { get; init; } = [];
    public Dictionary<string, string[]> C { get; init; } = [];
}