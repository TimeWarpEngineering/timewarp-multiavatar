#region Purpose
// Known-value checks for HashService.GenerateHash, the 12-digit hash every avatar is built from.
#endregion

#region Design
// Expected values were computed with the upstream JavaScript algorithm: SHA-256 of the UTF-8 input as hex,
// non-digits removed, first 12 digits kept.
#endregion

namespace TimeWarp.Multiavatar.Tests;

internal abstract class HashService_GenerateHash_Given_
{
  [ModuleInitializer]
  internal static void Register() => RegisterTests<HashService_GenerateHash_Given_>();

  [Input("Binx Bond", "798313128183")]
  [Input("1", "686273341968")]
  [Input("Ünïcødé 漢字 🙂", "436868057505")]
  public static Task Input_Should_Return_Upstream_Hash(string input, string expected)
  {
    HashService.GenerateHash(input).ShouldBe(expected);
    return Task.CompletedTask;
  }
}
