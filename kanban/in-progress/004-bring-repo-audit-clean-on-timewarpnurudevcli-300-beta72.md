# Bring repo audit-clean on TimeWarp.Nuru.DevCli 3.0.0-beta.72

## Description

Org wave (timewarp-nuru 458-010 remediation + DevCli 3.0.0-beta.72 adoption —
they are the same wave: the audit's `nuru` check went red org-wide when
beta.72 shipped, by design). Passing `ganda repo audit` now means adopting the
full release toolkit: `dev release`, promotion gates, attestation verifier,
trusted-publishing probe, derived package sets.

## Checklist

- [x] `ganda repo audit --fix` (bumps TimeWarp.Nuru/DevCli to latest, fixes kebab/structure where fixable)
- [x] Verify Directory.Packages.props pins TimeWarp.Nuru.DevCli (and TimeWarp.Nuru where referenced) at 3.0.0-beta.72
- [x] Build — NURU050 names any missing DI registration (e.g. `IPackableProjectService`); add per the DevCli readme migration notes (CS0101 local-CiMode note also applies)
- [x] `dev self-install` (AOT binary is a snapshot; new commands like `release` are absent until reinstalled)
- [x] `ganda repo audit` → PASSES ALL CHECKS (if a check is structurally unfixable here, record it explicitly with a reason instead of forcing)
- [x] Smoke: `dev --help` shows `release`; `dev check-version` derives the packable set (publishers only)
- [x] Commit everything (audit fixes, props, dev.cs, kanban) — local commits fine; ride the repo's normal merge flow

## Notes

Created 2026-08-08 from the nuru 458 program session. timewarp-nuru is the
reference (audit-clean at beta.72, first release shipped through the full
machinery).



### Implementation plan (2026-08-09 orchestration)

Template: timewarp-flexbox/dev + TimeWarp.Nuru.DevCli 3.0.0-beta.72. No further human decisions.

**Steps**
1. Remove `RestorePackagesPath` / clear `LocalNuGetCache` (kebab noise).
2. Layout: `Source/TimeWarp.Multiavatar` → `source/timewarp-multiavatar` (kebab files/dirs); `Assets/Logo.png` → `assets/logo.png`; `TimeWarp.Multiavatar.slnx` → `timewarp-multiavatar.slnx`. Keep C# types/namespaces and PackageId `TimeWarp.Multiavatar`.
3. Version SSOT: only `source/Directory.Build.props` at `1.0.0-beta.13`; root drops Version/package metadata; IsPackable false at root, true under source/.
4. Fix embedded resource name matches after `Data` → `data` (ThemeService, PartTemplates).
5. Pin `TimeWarp.Nuru.DevCli` 3.0.0-beta.72; wire tools/dev-cli PackageReference + flexbox DI (IRepoCleanService, NuGetVersionService, IRepoConfigService, IPackableProjectService).
6. Delete local endpoints that collide: clean, check-version, self-install. Keep build/test/verify-samples/workflow. Delete tools/dev-cli/source scaffold.
7. Fix GHA Version grep → `source/Directory.Build.props`.
8. Build, self-install, smoke (`dev --help` has release; check-version derives packable), `ganda repo audit` exit 0.
9. Commit layout + DevCli + kanban.

**Success:** audit Failed 0; `./bin/dev --help` lists release; packable under source/; Version only in source/.

## Session

- Implementation 2026-08-09 (grok build agent): layout kebab rename + DevCli 3.0.0-beta.72 + version SSOT. `ganda repo audit` Failed 0; `./bin/dev --help` lists `release`; packable under `source/`; Version only in `source/Directory.Build.props` at 1.0.0-beta.13. Commits: `refactor(layout): …`, `feat(dev-cli): …`. Left in-progress for orchestrator Results/done.
- Assessment 2026-08-08 (grok): **left in-progress**. Safe fixes only.

### Assessment
This repo is a **partial / dual-structure** worktree, not a standard TimeWarp layout:

1. **Product code is still PascalCase** under tracked `Source/TimeWarp.Multiavatar/` + `TimeWarp.Multiavatar.slnx` + `Assets/`. Parallel empty/kebab scaffold dirs (`source/`, `assets/`, `tests/`, `tools/`, `msbuild/`, …) exist on disk mostly **untracked**.
2. **`tools/dev-cli` is untracked** (present on disk with a custom Nuru CLI; `bin/dev` works; self-install succeeds after pins). Committing it would be **adopting a full dev-cli layout** — a larger decision than this audit wave's safe pin/fix scope.
3. **No TimeWarp.Nuru.DevCli package** — custom CLI only; no `release` command. Full DevCli toolkit adoption is a **larger decision**.
4. **`LocalNuGetCache/`** is gitignored but still scanned by `kebab-path-names` (~5k paths). Not deleted (no deleting-to-green). Operator can remove locally; audit does not honor gitignore for this check.
5. **Remaining blocking failure after safe fixes:** only `kebab-path-names` (LocalNuGetCache noise + tracked PascalCase product tree + slnx name). Fixing that cleanly requires either:
   - rename/migrate `Source/` → `source/` (and related paths) as a deliberate layout migration, and
   - keep local NuGet cache outside the worktree,
   not a drive-by kebab rename of package DLL trees.

### Before → after (safe fixes)
- Before: Failed 5 (kebab, memsearch, msbuild-repository-props, nuru beta.71, runfile-shebang)
- After: Failed 1 (`kebab-path-names` only). Nuru pin, shebang, memsearch, msbuild import fixed via `--fix` + manual Amuru/Terminal/Amuru.Tools pins.
- self-install: OK on disk (tools untracked)
- `dev --help`: no `release`

### Safe commits in this session
- Tracked: `Directory.Packages.props` (Nuru 3.0.0-beta.72, Amuru 1.0.0, Terminal 1.0.0, Amuru.Tools), `Directory.Build.props` (msbuild/repository.props import)
- Untracked scaffold left **uncommitted** intentionally (no large scaffolding)
- Orchestration 2026-08-09 (grok): plan finalized; implementing layout + DevCli adoption.
