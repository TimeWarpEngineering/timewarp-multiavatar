#region Purpose
// Executes the verify-only CI workflow, and skips it for a non-build pull request.
#endregion
#region Design
// Verify-only scaffold: self-install -> clean -> build -> test.
// Self-install uses the runfile because bin/dev is gitignored and missing on a
// fresh CI checkout. Later steps use ./bin/dev (bin/dev.exe on Windows).
// The first step is the shared PullRequestBuildClassifier (TimeWarp.Nuru.DevCli,
// timewarp-nuru 595). It prints its decision; a pull request that changes only
// declared non-build paths exits 0 without self-install, clean, build, or test.
// Push, release, dispatch, and local runs always build. Repo additions to the
// rule live in .timewarp/dev.jsonc "pullRequestBuild" (readme.md builds because
// it is packed into the NuGet package). No copy of the rule lives here.
// Repos that publish NuGet replace this file with the Nuru promotion pipeline.
// This scaffold does not pack, attest, or promote.
#endregion

namespace DevCli.Commands;

[NuruRoute("workflow", Description = "Execute full CI workflow")]
public sealed class WorkflowCommand : ICommand<Unit>
{
  public sealed class Handler : IRequestHandler<WorkflowCommand, Unit>
  {
    private readonly ITerminal Terminal;
    private CancellationToken Ct;
    private string RepoRoot = null!;
    private string DevBin = null!;

    public Handler(ITerminal terminal)
    {
      Terminal = terminal;
    }

    public async ValueTask<Unit> Handle(WorkflowCommand command, CancellationToken cancellationToken)
    {
      Ct = cancellationToken;

      if (!FindRepoRoot()) return Unit.Value;

      PullRequestBuildDecision decision = await PullRequestBuildClassifier.DecideAsync(RepoRoot, Ct);
      Terminal.WriteLine(decision.Message);
      if (!decision.RequiresBuild)
      {
        Terminal.WriteLine("\nWorkflow completed successfully!".Green());
        return Unit.Value;
      }

      Terminal.WriteLine("Pipeline: self-install -> clean -> build -> test");
      if (!await SelfInstallAsync()) return Unit.Value;
      if (!await RunStepAsync("clean", "Clean failed!")) return Unit.Value;
      if (!await RunStepAsync("build", "Build failed!")) return Unit.Value;
      if (!await RunStepAsync("test", "Tests failed!")) return Unit.Value;

      Terminal.WriteLine("\nWorkflow completed successfully!".Green());
      return Unit.Value;
    }

    private bool FindRepoRoot()
    {
      string? root = Git.FindRoot();
      if (root is null)
      {
        Terminal.WriteErrorLine("Error: could not find repository root.");
        Environment.ExitCode = 1;
        return false;
      }

      RepoRoot = root;
      string binaryName = OperatingSystem.IsWindows() ? "dev.exe" : "dev";
      DevBin = Path.Combine(RepoRoot, "bin", binaryName);
      Terminal.WriteLine("Starting CI workflow...");
      return true;
    }

    private async Task<bool> SelfInstallAsync()
    {
      int exitCode = await DotNet.Run()
        .WithFile("tools/dev-cli/dev.cs")
        .WithArguments("self-install")
        .WithWorkingDirectory(RepoRoot)
        .WithNoValidation()
        .RunAsync(Ct);

      if (exitCode != 0)
      {
        Terminal.WriteErrorLine("Self-install failed!".Red());
        Environment.ExitCode = exitCode;
        return false;
      }

      return true;
    }

    private async Task<bool> RunStepAsync(string subcommand, string failureMessage)
    {
      int exitCode = await Shell.Builder(DevBin)
        .WithArguments(subcommand)
        .WithNoValidation()
        .RunAsync(Ct);

      if (exitCode != 0)
      {
        Terminal.WriteErrorLine(failureMessage.Red());
        Environment.ExitCode = exitCode;
        return false;
      }

      return true;
    }
  }
}
