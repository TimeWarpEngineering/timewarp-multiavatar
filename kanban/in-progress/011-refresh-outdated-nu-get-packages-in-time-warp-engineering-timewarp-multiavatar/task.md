# Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-multiavatar

## Description

Refresh outdated NuGet packages in TimeWarpEngineering/timewarp-multiavatar.
Filter: (all)
Outdated:
  TimeWarp.Nuru 4.0.0-beta.12 -> 4.0.0-beta.13 (patch)
  TimeWarp.SourceGenerators 1.0.0-beta.14 -> 1.0.0-beta.16 (patch)
  TimeWarp.Nuru.DevCli 4.0.0-beta.12 -> 4.0.0-beta.13 (patch)
  TimeWarp.Jaribu.TestingPlatform 1.0.0-beta.15 -> 1.0.0-beta.16 (patch)
Edit Directory.Packages.props in this repo only.

Version: set <Version> in source/Directory.Build.props from 1.0.0-beta.14 to 1.0.0-beta.15 in this pull request. The prerelease suffix is -beta.N, never +beta.N. The wave releases it after merge.

## Goal (implementer restatement)

Bump the four outdated TimeWarp packages in the root `Directory.Packages.props`
(Nuru and Nuru.DevCli 4.0.0-beta.12 -> beta.13, SourceGenerators 1.0.0-beta.14 -> beta.16,
Jaribu.TestingPlatform 1.0.0-beta.15 -> beta.16) and bump the package `<Version>` in
`source/Directory.Build.props` from 1.0.0-beta.14 to 1.0.0-beta.15.

Acceptance criteria:
- Only those version strings change (no other package edits).
- `./bin/dev build` and `./bin/dev test` pass with the new packages.
- `ganda repo audit` passes.

## Session

- Minted by ganda task wave run 20261114T054552Z-9a0f39
- 2026-11-14 implement: bumped the four listed pins plus `TimeWarp.Terminal` 2.0.0-beta.1 -> 2.0.0-beta.2
  (the first `ganda repo audit` failed `package-pin-currency` on it; boy-scout fix). Version 1.0.0-beta.14 -> 1.0.0-beta.15.
- 2026-11-14 implement: the first commit's post-commit attest failed `package-pin-currency` on the dev-only
  `TimeWarp.Build.Tasks` 2.0.0 pin (the commit moved the last-meaningful-commit date past the 2.0.1-beta.1
  publish date, ending the grace); bumped it to 2.0.1-beta.1.
- Implement oracle: implemented by implementer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session aa4d7012-89ae-4df7-81b8-acca229897e9, max-turns 500, Done — 2026-11-14T11:10:15Z

## Results

Changes:
- `Directory.Packages.props`: TimeWarp.Nuru 4.0.0-beta.13, TimeWarp.Nuru.DevCli 4.0.0-beta.13,
  TimeWarp.SourceGenerators 1.0.0-beta.16, TimeWarp.Jaribu.TestingPlatform 1.0.0-beta.16,
  TimeWarp.Terminal 2.0.0-beta.2 and TimeWarp.Build.Tasks 2.0.1-beta.1 (audit/attest `package-pin-currency` fixes).
- `source/Directory.Build.props`: `<Version>` 1.0.0-beta.15.

### Gates

- `./bin/dev build` — passed, 0 warnings, 0 errors.
- `./bin/dev test` — passed, 24/24 succeeded.
- `ganda repo audit` — first run failed `package-pin-currency` (TimeWarp.Terminal 2.0.0-beta.1 behind beta.2);
  post-commit attest then failed the same check on TimeWarp.Build.Tasks 2.0.0 (behind 2.0.1-beta.1). After both
  bumps, build and test were rerun (pass, 0 warnings, 24/24) and the audit rerun passed (exit 0) with 2 advisory
  warnings left as-is: `memsearch-scaffold` (outdated .githooks) and `vscode-window-icon` (missing .vscode files),
  both non-blocking and outside this package-refresh scope.

### How to validate

Smoke:
```bash
./bin/dev build && ./bin/dev test && ganda repo audit
```

Expect: build succeeds with 0 errors; 24 tests pass; audit reports "Repository passes" and
`package-pin-currency` shows every TimeWarp pin as the latest.
