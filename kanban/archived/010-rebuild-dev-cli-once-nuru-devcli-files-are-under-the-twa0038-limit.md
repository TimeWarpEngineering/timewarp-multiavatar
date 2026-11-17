# Rebuild dev CLI once Nuru DevCli files are under the TWA0038 limit

## Description

Since task 009 (TimeWarp.SourceGenerators 1.0.0-beta.14), `tools/dev-cli` no longer builds from source.
`dotnet run tools/dev-cli/dev.cs -- self-install` (and `ganda repo audit --fix` for `bin-dev`) fails:

```
~/.nuget/packages/timewarp.nuru.devcli/4.0.0-beta.12/content/any/endpoints/release-command.cs(72,1): error TWA0038: File 'release-command.cs' has 609 lines, over the 500-line limit
~/.nuget/packages/timewarp.nuru.devcli/4.0.0-beta.12/content/any/services/nuget-version-service.cs(73,1): error TWA0038: File 'nuget-version-service.cs' has 704 lines, over the 500-line limit
```

Both files are TimeWarp.Nuru.DevCli source content, outside this repo, so they cannot be split here.
The master `bin/dev` was published before task 009 and still works. A fresh worktree has no `bin/dev`,
so the `bin-dev` and `dev-cli-capabilities` audit checks fail there. Task 008 worked around it by copying
master's `bin/dev` into its worktree (the dev-cli source is unchanged since that binary was built).

Upstream fix: timewarp-nuru kanban tasks **576** and **577** (split DevCli `release-command.cs` and
`nuget-version-service.cs` under the 500-line TWA0038 cap).

## Requirements

- Take the TimeWarp.Nuru / TimeWarp.Nuru.DevCli release that contains the Nuru 576/577 split.
- `dotnet run tools/dev-cli/dev.cs -- self-install` succeeds from a clean worktree.
- No TWA0038 suppression or `max_lines` override added in this repo.
- `ganda repo audit` passes `bin-dev` and `dev-cli-capabilities` in a fresh task worktree.

## Checklist

- [ ] Nuru 576/577 released; note the DevCli version here
- [ ] Bump TimeWarp.Nuru and TimeWarp.Nuru.DevCli in `Directory.Packages.props`
- [ ] dev CLI self-install succeeds from source
- [ ] `ganda repo audit` clean in a fresh worktree

## Session

- Filed from task 008 (2026-11-12)

## Notes

- Blocked by timewarp-nuru tasks 576/577 (other repo, so not a `## Depends on` id here).
- Can be combined with task 007 if the same DevCli release also ships the CA1812 fix.
