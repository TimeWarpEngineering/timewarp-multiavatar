# Update TimeWarp.Build.Tasks and TimeWarp.SourceGenerators to latest

## Description

Bump TimeWarp.Build.Tasks 1.0.0 -> 2.0.0 and TimeWarp.SourceGenerators 1.0.0-beta.11 -> 1.0.0-beta.14, and fix the diagnostics the newer analyzers raise (TWA0004: every source file needs a `#region Purpose` block).

## Checklist

- [x] Bump both packages in Directory.Packages.props
- [x] Add Purpose regions (and Design where a real design point exists) to the 9 flagged files
- [x] `bin/dev build` clean (0 warnings / 0 errors)
- [x] `ganda repo audit` has no Error failures
- [x] `dotnet pack -c Release` works; package contents unchanged
- [x] `ganda nuget outdated` reports nothing outdated
- [x] Smoke test: Generate("hello") returns SVG of length 4722

## Session

- Created: b2020e9f-5c06-487a-a0b3-831ae1639f80 (2026-11-12)

## Results

- Only diagnostic after the bump was TWA0004 (9 files); fixed with Purpose regions, plus Design regions on 6 files. No code, API or suppression changes.
- Build: 0 warnings / 0 errors. Audit: 41 passed, 2 warnings (ci-code-coverage, vscode-window-icon), 0 errors.
- Pack: nuspec has one dependency group (net11.0: TimeWarp.Terminal 2.0.0-beta.1); package holds the dll, logo, readme, as before.
- Build.Tasks 2.0.0: task assembly targets net11.0 only; builds under VS/MSBuild.exe or older SDKs stop with TWBT0001 (git metadata CommitHash/CommitDate still injected).
- Smoke test: Generate("hello") returned 4722 characters.
