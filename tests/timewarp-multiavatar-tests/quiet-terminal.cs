#region Purpose
// Routes the library's TimeWarp.Terminal debug traces to an in-memory terminal so they do not flood the test log.
#endregion

#region Design
// The library writes per-part DEBUG lines to stderr through the static Terminal facade. A module initializer
// swaps Terminal.Instance once, before any test runs. No test reads or asserts on that output.
#endregion

namespace TimeWarp.Multiavatar.Tests;

internal static class QuietTerminal
{
  [ModuleInitializer]
  internal static void Install() => TimeWarp.Terminal.Terminal.Instance = new TimeWarp.Terminal.TestTerminal();
}
