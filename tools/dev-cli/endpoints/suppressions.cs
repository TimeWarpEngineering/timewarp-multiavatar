#region Purpose
// CA1812 suppressions for TimeWarp.Nuru.DevCli content handlers
#endregion
#region Design
// The Nuru mediator activates these internal handlers, so CA1812 cannot see an instantiation.
// They are declared in TimeWarp.Nuru.DevCli content files this repo cannot edit, so the
// suppressions are scoped to exact types here (same pattern as timewarp-amuru).
#endregion

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
  "Performance",
  "CA1812",
  Justification = "Activated by the Nuru mediator. Declared in TimeWarp.Nuru.DevCli content, which this repo cannot edit.",
  Scope = "type",
  Target = "~T:DevCli.ReleaseCommand.Handler")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
  "Performance",
  "CA1812",
  Justification = "Activated by the Nuru mediator. Declared in TimeWarp.Nuru.DevCli content, which this repo cannot edit.",
  Scope = "type",
  Target = "~T:DevCli.SelfInstallCommand.Handler")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
  "Performance",
  "CA1812",
  Justification = "Activated by the Nuru mediator. Declared in TimeWarp.Nuru.DevCli content, which this repo cannot edit.",
  Scope = "type",
  Target = "~T:DevCli.CheckVersionCommand.Handler")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
  "Performance",
  "CA1812",
  Justification = "Activated by the Nuru mediator. Declared in TimeWarp.Nuru.DevCli content, which this repo cannot edit.",
  Scope = "type",
  Target = "~T:DevCli.CleanCommand.Handler")]
