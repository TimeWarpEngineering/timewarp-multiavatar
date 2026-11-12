#region Purpose
// Guards the embedded SVG-template and theme resources: every avatar type's parts and every theme resolve.
#endregion

#region Design
// A folder or resource rename leaves the loaders with nothing to find and avatars silently lose parts, so
// this loops every type, part and variant once instead of testing per avatar.
#endregion

namespace TimeWarp.Multiavatar.Tests;

internal abstract class EmbeddedResources_Load_Given_
{
  private static readonly string[] PartNames = ["env", "clo", "head", "mouth", "eyes", "top"];
  private static readonly string[] Variants = ["A", "B", "C"];

  [ModuleInitializer]
  internal static void Register() => RegisterTests<EmbeddedResources_Load_Given_>();

  public static Task EveryAvatarType_Should_Resolve_All_Parts()
  {
    for (int type = 0; type < 16; type++)
    {
      string avatarType = type.ToString("D2", System.Globalization.CultureInfo.InvariantCulture);
      Dictionary<string, string> parts = PartTemplates.GetAllParts(avatarType);

      foreach (string partName in PartNames)
      {
        parts.ShouldContainKey(partName, $"avatar type {avatarType}");
        PartTemplates.GetPart(avatarType, partName).ShouldNotBeNullOrWhiteSpace($"{avatarType}/{partName}");
      }
    }

    return Task.CompletedTask;
  }

  public static Task EveryTheme_Should_Resolve_All_Variants_And_Parts()
  {
    for (int type = 0; type < 16; type++)
    {
      string themeNumber = type.ToString("D2", System.Globalization.CultureInfo.InvariantCulture);
      ThemeService.GetTheme(themeNumber).ShouldNotBeNull($"theme {themeNumber}");

      foreach (string variant in Variants)
      {
        foreach (string partName in PartNames)
        {
          ThemeService.GetThemeColors(themeNumber, variant, partName).ShouldNotBeEmpty($"{themeNumber}/{variant}/{partName}");
        }
      }
    }

    return Task.CompletedTask;
  }
}
