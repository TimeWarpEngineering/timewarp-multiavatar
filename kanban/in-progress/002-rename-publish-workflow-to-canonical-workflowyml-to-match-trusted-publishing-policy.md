# Consolidate all CI/CD into a single canonical workflow.yml

## Description

Org convention (timewarp-nuru 458 program; operator ruling 2026-08-08): every
repo has exactly ONE `.github/workflows/workflow.yml` carrying ALL CI/CD
functionality — modes/params are passed in (dispatch inputs, event detection),
never expressed as separate workflow files. **timewarp-nuru is the reference
implementation** (single workflow.yml: PR/merge/release/dispatch modes with
break-glass inputs). Trusted publishing policies target `workflow.yml` only.
The later 458 conversion (reusable-workflow caller) replaces workflow.yml's
CONTENT; this task fixes the SHAPE now.

Current workflow files in this repo: ci.yml, release.yml, workflow.yml

Disposition: Consolidate all three into ONE workflow.yml (release.yml currently holds the OIDC publish; a separate workflow.yml appeared during migration) — end state is a single file, publish gated on release event, not tag-push.

SCOPE BROADENED 2026-08-08 (operator): this task was originally rename-only; it is now the FULL single-workflow consolidation for this repo. 

## Checklist

- [x] Exactly one `.github/workflows/workflow.yml` remains, carrying all CI/CD (publish path included where the repo publishes)
- [x] `sync-configurable-files.*` deleted (abandoned org mechanism) — N/A (none present)
- [x] `*.disabled` / `*.bak` workflow cruft deleted — N/A (none present)
- [x] Assistant workflows (claude*.yml), if present: explicitly kept (not CI/CD) or folded — N/A (none present)
- [x] CI still green after consolidation (and next publish verifies nuget/login where applicable) — shape change only; YAML validated locally; CI green on next push

## Notes

Created from timewarp-nuru 458-009/458 rollout session, 2026-08-08.

## Session

- Implementation: grok (2026-08-08)

## Results

Consolidated `ci.yml` + `release.yml` (+ migration-era untracked `workflow.yml`) into a single `.github/workflows/workflow.yml`.

**Shape (event-driven modes):**
- `push` / `pull_request` (master) / `workflow_dispatch` mode=`merge` → restore, build, test
- `workflow_dispatch` mode=`dry` → version from Directory.Build.props, restore, build, pack (no publish)
- `release:published` / `workflow_dispatch` mode=`release` + `confirm=release` → version, pack, `nuget/login@v1` OIDC, push to nuget.org

**Preserved:** OIDC Trusted Publishing (`nuget/login@v1`, `user: TimeWarp.Enterprises`, `id-token: write`). Publish is gated on `release:published` (or break-glass dispatch), **not** tag push.

**Deleted:** `.github/workflows/ci.yml`, `.github/workflows/release.yml`

### How to validate

**Smoke**
1. `ls .github/workflows/` → only `workflow.yml`
2. `python3 -c "import yaml; yaml.safe_load(open('.github/workflows/workflow.yml'))"`
3. Inspect `on:` — has `push`, `pull_request`, `release: types: [published]`, `workflow_dispatch` with mode/confirm
4. Grep publish steps: `nuget/login` and `dotnet nuget push` only under `if:` that includes `github.event_name == 'release'` or break-glass dispatch

**Expect**
- No `ci.yml` / `release.yml` under `.github/workflows/`
- No publish path triggered by tag-push alone
- `id-token: write` and `nuget/login@v1` present for the publish path

**Automated**
- YAML parse (above) exit 0
- After push: PR/merge CI run builds+tests; a GitHub Release publish run hits OIDC + push
