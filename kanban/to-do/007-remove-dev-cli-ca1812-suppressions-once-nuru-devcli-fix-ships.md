# Remove dev-cli CA1812 suppressions once Nuru DevCli fix ships

## Description

Task 006 added four assembly-level CA1812 `[SuppressMessage]` attributes in
`tools/dev-cli/endpoints/suppressions.cs` and raised `.timewarp/suppression-baseline.json`
(`"tools/dev-cli/endpoints/suppressions.cs": { "CA1812": 4 }`) to match.

They exist only because TimeWarp.Nuru.DevCli 4.0.0-beta.12 ships four `internal sealed class Handler`
types as source content (Release, SelfInstall, CheckVersion, Clean). The mediator activates them, so
CA1812 fires in this repo's dev-cli build and this repo cannot edit the package's files.

The upstream fix is timewarp-nuru kanban task **573** ("DevCli internal Handler classes trip CA1812 in
consumer projects"). This task is blocked until a TimeWarp.Nuru.DevCli release contains that fix.

## Requirements

- Take the TimeWarp.Nuru / TimeWarp.Nuru.DevCli version that includes Nuru 573.
- Delete `tools/dev-cli/endpoints/suppressions.cs`.
- Remove the `tools/dev-cli/endpoints/suppressions.cs` entry from `.timewarp/suppression-baseline.json`.
- `bin/dev build` ends with 0 warnings and 0 errors, with no CA1812 suppression anywhere in the repo.
- `ganda repo audit` passes `analyzer-suppressions`.

## Checklist

- [ ] Nuru 573 merged and released; note the DevCli version here
- [ ] Bump TimeWarp.Nuru and TimeWarp.Nuru.DevCli in `Directory.Packages.props`
- [ ] Delete `tools/dev-cli/endpoints/suppressions.cs`
- [ ] Remove its entry from `.timewarp/suppression-baseline.json`
- [ ] `bin/dev build` clean
- [ ] `ganda repo audit` clean

## Session

- Created: b2020e9f-5c06-487a-a0b3-831ae1639f80 (2026-11-12)

## Notes

- Blocked by timewarp-nuru task 573 (other repo, so it cannot be a `## Depends on` id here).
- If the released fix still leaves any CA1812 in the generated dev CLI, report it on Nuru 573 instead of
  keeping a suppression.
