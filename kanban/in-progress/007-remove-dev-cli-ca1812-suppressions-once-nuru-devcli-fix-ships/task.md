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

## Goal (implementer restatement)

Drop the four consumer-side CA1812 `[SuppressMessage]` attributes that task 006 added for the
TimeWarp.Nuru.DevCli content handlers, now that Nuru 573 makes the generated mediator construct
those handlers with an explicit `new` (so CA1812 sees an instantiation).

Acceptance:
- The pinned TimeWarp.Nuru / TimeWarp.Nuru.DevCli version contains the Nuru 573 fix.
- `tools/dev-cli/endpoints/suppressions.cs` is gone and its entry is gone from
  `.timewarp/suppression-baseline.json`; no CA1812 suppression remains anywhere in the repo.
- `./bin/dev build` reports 0 warnings and 0 errors; `./bin/dev test` passes.
- `ganda repo audit` passes, including `analyzer-suppressions`.

## Checklist

- [x] Nuru 573 merged and released; note the DevCli version here — first shipped in 4.0.0-beta.14 (`land kanban 573` commit 4efa3f21 is in tags v4.0.0-beta.14 and v4.0.0-beta.15)
- [x] Bump TimeWarp.Nuru and TimeWarp.Nuru.DevCli in `Directory.Packages.props` — already at 4.0.0-beta.15 on master (bumped by an earlier task), no change needed
- [x] Delete `tools/dev-cli/endpoints/suppressions.cs`
- [x] Remove its entry from `.timewarp/suppression-baseline.json`
- [x] `bin/dev build` clean
- [x] `ganda repo audit` clean

## Session

- Created: b2020e9f-5c06-487a-a0b3-831ae1639f80 (2026-11-12)
- 2026-11-17 implement oracle (ganda task work, implementer-claude): Nuru 573 is in DevCli
  4.0.0-beta.14+, and this repo already pins 4.0.0-beta.15. Deleted the suppression file and its
  baseline entry; dev CLI re-published and all gates pass.
- Implement oracle: implemented by implementer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session f3de8c53-7b2f-41ad-8d6b-bbedf7b6f07d, max-turns 500, Done — 2026-11-17T10:42:23Z
- Review oracle: grok (vendor xai), session 01a1e919-e7ed-7540-bc00-a6f114cab862, effort 1, roster general, round 1, disposition clean — 2026-11-17
- Review oracle: review by reviewer-grok (grok, vendor xai, model grok-4.7, response model grok-4.7-build), session 01a1e919-e7ed-7540-bc00-a6f114cab862, max-turns 80, artifacts general.md, merged.md, disposition.md — 2026-11-17T10:50:23Z

## Review

- Round 1: Done

## Notes

- Blocked by timewarp-nuru task 573 (other repo, so it cannot be a `## Depends on` id here).
- If the released fix still leaves any CA1812 in the generated dev CLI, report it on Nuru 573 instead of
  keeping a suppression.

## Results

Nuru 573 fixed the CA1812 hits in the generator. The generated mediator now builds each nested
DevCli `Handler` with an explicit `new` (`__UseHandlerFactory<H>(...)`), so CA1812 sees the
instantiation, and the shipped content stays `internal sealed class Handler`. That fix first
shipped in TimeWarp.Nuru / TimeWarp.Nuru.DevCli 4.0.0-beta.14. `Directory.Packages.props` already
pins both at 4.0.0-beta.15, so no version bump was needed.

Changes:
- Deleted `tools/dev-cli/endpoints/suppressions.cs` (the four assembly-level CA1812 `[SuppressMessage]`).
- Removed the `tools/dev-cli/endpoints/suppressions.cs` entry (`CA1812: 4`) from
  `.timewarp/suppression-baseline.json`.
- No CA1812 suppression is left in the repo outside `kanban/` (grep over `*.cs`, `*.props`,
  `*.csproj`, `*.json`, `.editorconfig` finds none).

### Gates

- `dotnet run --file tools/dev-cli/dev.cs -- self-install` (once, because `tools/dev-cli/` changed):
  passed. The dev CLI published under `TreatWarningsAsErrors` + `AnalysisMode All` with no CA1812.
- `./bin/dev build`: passed, 0 Warning(s), 0 Error(s).
- `./bin/dev test`: passed, 24 total, 24 succeeded, 0 failed.
- `ganda repo audit`: exit 0, and `analyzer-suppressions` passes. The only FAIL row is
  `vscode-window-icon` (Warning severity). It reports `.vscode/settings.json` and `.vscode/tasks.json`
  missing. `.vscode/` is gitignored (`.gitignore:25`) and is local-only: the origin-home checkout has
  it, a fresh task worktree does not. There is nothing to commit for it.

### How to validate

Smoke:
```bash
test ! -e tools/dev-cli/endpoints/suppressions.cs && echo "suppressions file gone"
grep -c "suppressions.cs" .timewarp/suppression-baseline.json
grep -rn "CA1812" --include=*.cs --include=*.props --include=*.csproj --include=*.json --include=.editorconfig . | grep -v kanban/
dotnet run --file tools/dev-cli/dev.cs -- self-install
./bin/dev build
ganda repo audit
```
Expect: "suppressions file gone"; the baseline grep prints `0`; the CA1812 grep prints nothing; the
self-install publishes the dev CLI with no CA1812 error; the build ends with `0 Warning(s)` /
`0 Error(s)`; and the audit exits 0 with `analyzer-suppressions` passing. A local
`vscode-window-icon` warning can show up in a worktree that has no `.vscode/`.

### Review

Round 1, effort 1, reviewer roster: general. Counts: bug 0 open / 0 fixed / 0 wontfix, suggestion 0, nit 0. Final open count 0. Disposition: clean. The implementer restatement matches the original task, so there is no misread under Learnings. Paths: `review/review-framework.md`, `review/round-1/general.md`, `review/round-1/merged.md`, `review/disposition.md`.
