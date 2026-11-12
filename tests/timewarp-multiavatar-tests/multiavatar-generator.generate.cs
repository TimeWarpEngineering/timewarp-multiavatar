#region Purpose
// Regression tests for MultiavatarGenerator.Generate: equivalence with the upstream JavaScript generator,
// determinism, sansEnv, the ver override and edge inputs.
#endregion

#region Design
// Expected SVGs are checked-in output of upstream @multiavatar/multiavatar (see references/readme.md) and are
// compared byte for byte. A mismatch means this port differs from the web version: record it, do not edit
// the reference to match.
#endregion

namespace TimeWarp.Multiavatar.Tests;

internal abstract class MultiavatarGenerator_Generate_Given_
{
  private const string SvgStart = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 231 231\">";

  [ModuleInitializer]
  internal static void Register() => RegisterTests<MultiavatarGenerator_Generate_Given_>();

  [Input("binx-bond", "Binx Bond")]
  [Input("starcrasher", "Starcrasher")]
  [Input("leia", "Leia")]
  [Input("alice", "alice")]
  [Input("bob", "bob")]
  [Input("digit-one", "1")]
  [Input("timewarp-multiavatar", "timewarp-multiavatar")]
  public static Task Input_Should_Equal_Upstream_Reference(string referenceName, string input)
  {
    MultiavatarGenerator.Generate(input).ShouldBe(ReferenceSvg.Load(referenceName));
    return Task.CompletedTask;
  }

  public static Task NonAsciiInput_Should_Equal_Upstream_Reference()
  {
    MultiavatarGenerator.Generate("Ünïcødé 漢字 🙂").ShouldBe(ReferenceSvg.Load("non-ascii"));
    return Task.CompletedTask;
  }

  public static Task EmptyInput_Should_Return_Empty_String()
  {
    MultiavatarGenerator.Generate(string.Empty).ShouldBe(string.Empty);
    return Task.CompletedTask;
  }

  public static Task SameInput_Should_Return_Same_Svg()
  {
    string first = MultiavatarGenerator.Generate("Binx Bond");
    _ = MultiavatarGenerator.Generate("Starcrasher");
    string second = MultiavatarGenerator.Generate("Binx Bond");

    second.ShouldBe(first);
    return Task.CompletedTask;
  }

  public static Task SansEnv_Should_Omit_Environment_And_Otherwise_Match()
  {
    string full = MultiavatarGenerator.Generate("Binx Bond");
    string sansEnv = MultiavatarGenerator.Generate("Binx Bond", sansEnv: true);

    sansEnv.ShouldBe(ReferenceSvg.Load("binx-bond.sans-env"));
    sansEnv.ShouldStartWith(SvgStart);
    sansEnv.Length.ShouldBeLessThan(full.Length);
    full.ShouldEndWith(sansEnv[SvgStart.Length..]);
    return Task.CompletedTask;
  }

  public static Task VerOverride_Should_Force_Part_And_Theme()
  {
    Dictionary<string, string> ver = new() { ["part"] = "11", ["theme"] = "C" };

    string svg = MultiavatarGenerator.Generate("Binx Bond", ver: ver);

    svg.ShouldBe(ReferenceSvg.Load("binx-bond.ver-11c"));
    svg.ShouldNotBe(MultiavatarGenerator.Generate("Binx Bond"));
    return Task.CompletedTask;
  }
}
