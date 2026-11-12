#region Purpose
// Known-value checks for HashService.GetPartSelection, which maps each 2-digit hash slice to a part and theme.
#endregion

#region Design
// Expected values were computed with the upstream JavaScript algorithm: Math.round(47 / 100 * n), split into
// variant A below 16, B below 32 and C from 32. "50" is the one value that lands exactly on .5.
#endregion

namespace TimeWarp.Multiavatar.Tests;

internal abstract class HashService_GetPartSelection_Given_
{
  [ModuleInitializer]
  internal static void Register() => RegisterTests<HashService_GetPartSelection_Given_>();

  [Input("00", "00A")]
  [Input("15", "07A")]
  [Input("34", "00B")]
  [Input("50", "08B")]
  [Input("67", "15B")]
  [Input("68", "00C")]
  [Input("99", "15C")]
  public static Task TwoDigits_Should_Return_Upstream_Selection(string digits, string expected)
  {
    HashService.GetPartSelection(digits).ShouldBe(expected);
    return Task.CompletedTask;
  }
}
