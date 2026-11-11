# Bring repo to audit baseline

## Description

Bring timewarp-multiavatar to the `ganda repo audit` baseline: apply `--fix`, finish the kebab-case renames and stale path references, convert the workflow to pwsh, and keep the build green.

## Checklist

- [x] Run `ganda repo audit --fix` and keep the baseline (Nuru 4.0.0-beta.12, dev-cli, msbuild/repository.props, .editorconfig, hooks, gitignore entries)
- [x] Kebab-case renames: source/, assets/logo.png, csproj, slnx, folders and .cs files
- [x] Repair stale path references (slnx, csproj, embedded resource names, readme pack item, workflow)
- [x] Convert workflow to `defaults.run.shell: pwsh` and port `Resolve version` to PowerShell
- [x] Move `<Version>` to source/Directory.Build.props (workflow reads it there)
- [x] Fix analyzer diagnostics in code; build the dev CLI and the library
- [x] Re-run audit
- [ ] analyzer-suppressions: 4 CA1812 suppressions for TimeWarp.Nuru.DevCli handlers (open, see Results)

## Session

- Created: b2020e9f-5c06-487a-a0b3-831ae1639f80 (2026-11-11)

## Notes

## Results

Audit after this task: 38 passed, 2 failed (1 Error, 1 Warning), 6 skipped.

Fixed:
- Applied `ganda repo audit --fix` baseline; Nuru 2.1.0-beta.17 -> 4.0.0-beta.12.
- Renamed `Source/` -> `source/`, `Assets/Logo.png` -> `assets/logo.png`, the csproj and slnx, `Data/Models/Services` folders and all .cs files to kebab-case (git mv). `avatar-part.cs` became `avatar-part-selection.cs` because the audit rejects a `-part` suffix. The class `AvatarPart` is unchanged.
- Embedded resource lookups now use `.data.` (folder renamed). The csproj pins `AssemblyName` and `RootNamespace` to `TimeWarp.Multiavatar`. Verified a generated avatar and `dotnet pack` (logo.png, readme.md, package version 1.0.0-beta.13).
- `<Version>` moved to source/Directory.Build.props; the workflow reads it there.
- Workflow: `defaults.run.shell: pwsh`, `Resolve version` ported to PowerShell, `feature/**` added to pull_request branches.
- Removed `RestorePackagesPath=LocalNuGetCache` from root props. The in-repo cache made the audit scan third-party .cs files (line cap, editorconfig).
- Library analyzer fixes: collection expressions, explicit types. The four stderr `DEBUG` traces were first dropped (`Error.WriteLine`, banned Console), then restored via `TimeWarp.Terminal.Terminal.WriteErrorLine` (fully qualified because `Terminal` resolves to the `TimeWarp.Terminal` namespace inside `TimeWarp.Multiavatar`).
- Library retargeted net10.0 -> net11.0 (TimeWarp.Terminal 2.0.0-beta.1 ships net11.0 only). Removed the now-duplicate `.githooks/Directory.Build.props` and the dev-cli TFM override; CI `setup-dotnet` now `11.0.x`. Breaking for net10.0 consumers.
- dev-cli: handlers made public, `ct` renamed `cancellationToken`, blank lines for IDE2003.

Open:
- `analyzer-suppressions` (Error): 4 CA1812 suppressions in tools/dev-cli/endpoints/suppressions.cs for internal handlers declared in TimeWarp.Nuru.DevCli content that this repo cannot edit.
- `ci-code-coverage` (Warning): no coverage collection; the repo has no tests yet.
