#region Purpose
// Loads a checked-in upstream reference SVG from the test output directory.
#endregion

namespace TimeWarp.Multiavatar.Tests;

internal static class ReferenceSvg
{
  internal static string Load(string name) =>
    File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "references", name + ".svg"));
}
