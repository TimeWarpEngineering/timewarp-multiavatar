# Migrate NuGet publish workflow to trusted publishing (nuget/login)

## Description

The trusted publishing policy for this repo already exists on NuGet.org
(owner TimeWarp.Enterprises, created 2026-08-08) but is INERT until the
publish workflow exchanges an OIDC token for a temp key instead of using a
stored secret. Org program context: timewarp-nuru kanban 458-009.

Current state (2026-08-07 org audit): secret `NUGET_API_KEY` in release.yml; publishes on ANY tag push (`tags: ['*']`) and on dispatch with no gate — consider moving to the release:published event while in here.

Reference implementation: timewarp-nuru `.github/workflows/workflow.yml` —
`nuget/login@v1` step (user: TimeWarp.Enterprises) gated on the release
condition, `id-token: write` job permission, push via
`--api-key ${{ steps.nuget-login.outputs.NUGET_API_KEY }}`.

NOTE: if this repo's full convention conversion (reusable-workflow caller,
timewarp-nuru 458 rollout) is imminent, do the conversion instead — it
includes this migration for free.

## Checklist

- [x] Add `id-token: write` (with `contents: read`) permissions to the publish job
- [x] Add `nuget/login@v1` gated on the publish condition
- [x] Replace the stored-secret `--api-key` with the login step output
- [ ] Verify the publish path end-to-end on the next release
- [ ] AFTER verified: operator revokes the long-lived NuGet key and deletes the GitHub secret (org-wide revocation tracked in nuru 458-009)

## Notes

Created from the timewarp-nuru 458-009 rollout session (2026-08-08).

## Implementation Plan

### Decisions (locked)

1. **Migrate `release.yml` only** — do NOT do full 458 convention conversion now.
   - Untracked workflow.yml is CI-only with no pack/push.
   - Full conversion is multi-day; this task is the interim security fix.
   - When full 458 conversion lands later, re-bind NuGet TP policy to the new workflow filename.

2. **Triggers:** remove any-tag push and ungated dispatch as publish paths.
   - Primary: `release: types: [published]`
   - Keep gated break-glass `workflow_dispatch` with `mode` + `confirm` (nuru-shaped).

3. **Keep filename `release.yml`** — NuGet TP policy is name-sensitive; renaming breaks OIDC until policy is updated.

### In-scope
- Edit only `.github/workflows/release.yml`
- permissions: contents: read + id-token: write
- nuget/login@v1 user TimeWarp.Enterprises, gated on publish condition
- Push via steps.nuget-login.outputs.NUGET_API_KEY (no secrets.NUGET_API_KEY)
- Version from release.tag_name or Directory.Build.props for dispatch

### Out of scope
- Full 458 conversion, ci.yml / untracked workflow.yml changes
- Secret revoke / key revoke (operator after E2E)
- NuGet.org policy UI edits (already done unless filename mismatch)

### Target YAML shape
- on: release published + workflow_dispatch with mode dry|release and confirm
- Job permissions id-token write
- Validate break-glass if mode=release without confirm=release
- Resolve version from github.event.release.tag_name or props
- nuget/login + push only when release event OR break-glass confirmed
- No secrets.NUGET_API_KEY fallback

### Validation
- Static: no secret key refs; login/push same if; id-token present
- Dry dispatch: build/pack only, login skipped
- Real release E2E is operator on next publish
- After green: operator deletes secret; key revoke coordinated with nuru 458-009 if shared

### Files
| File | Action |
|------|--------|
| .github/workflows/release.yml | Only code change |
| kanban task 001 | Checklist + notes |

## Session

- Orchestrator: grok (2026-08-08)
- Plan: finalized migrate release.yml only + release:published + nuget/login
- Implementation: grok (2026-08-08)
