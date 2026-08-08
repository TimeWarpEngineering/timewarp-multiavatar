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
- Review: grok Phase 4b effort-1 (2026-08-08) — disposition clean

## Results

### What was implemented
Migrated `.github/workflows/release.yml` from long-lived `secrets.NUGET_API_KEY` to NuGet Trusted Publishing via OIDC (`nuget/login@v1`).

- Job permissions: `contents: read`, `id-token: write`
- Primary trigger: `release: types: [published]` (removed any-tag push)
- Break-glass: `workflow_dispatch` with `mode` dry|release and `confirm=release` guard
- Version: `github.event.release.tag_name` on release events; `Directory.Build.props` `<Version>` on dispatch
- Auth: `nuget/login@v1` user `TimeWarp.Enterprises`; push uses `steps.nuget-login.outputs.NUGET_API_KEY` only
- Login and push share the same publish `if:` condition (dry dispatch packs only)

### Files changed
| File | Change |
|------|--------|
| `.github/workflows/release.yml` | OIDC trusted-publishing migration + trigger/version gates |
| `kanban/.../001-.../task.md` | Plan, checklist, session, results |
| `kanban/.../001-.../review/**` | Phase 4b framework, round-1 general+merged, disposition |

### Key decisions / deviations
- Migrated `release.yml` only — full 458 convention conversion deferred (WIP CI-only `workflow.yml` has no pack/push)
- Kept filename `release.yml` (NuGet TP policy is name-sensitive)
- No fallback to `secrets.NUGET_API_KEY`
- No deviations from the locked plan

### Test outcomes
- Static grep: no `secrets.NUGET_*` under `.github/workflows/`
- Phase 4b review: effort 1, roster `general`, 1 round, **0 open** (0 bug / 0 suggestion / 0 nit)
- Disposition: **clean** — see `review/disposition.md`, `review/round-1/merged.md`

### Residual operator work (not implementer)
Checklist items still open by design (require live Actions + NuGet.org):
1. E2E verify on next real GitHub Release publish
2. After green: delete GitHub secret `NUGET_API_KEY` on this repo; revoke long-lived key only when safe (org-wide: nuru **458-009** if key is shared)

### Phase 4b facts
- Rounds: 1
- Effort / roster: 1 / general
- Final counts: all zero open
- Outcome: clean
- Paths: `review/review-framework.md`, `review/round-1/general.md`, `review/round-1/merged.md`, `review/disposition.md`

### How to validate

**Smoke (static, from repo root)**
```bash
# Expect: no stored NuGet secret references
rg -n 'secrets\.(NUGET|PUBLISH)' .github/workflows/ || true
# expect: no matches

# Expect: OIDC login + id-token permission present
rg -n 'nuget/login@v1|id-token: write|TimeWarp\.Enterprises' .github/workflows/release.yml
# expect: all three patterns match

# Expect: push uses login step output, not secrets
rg -n 'steps\.nuget-login\.outputs\.NUGET_API_KEY|secrets\.NUGET' .github/workflows/release.yml
# expect: steps.nuget-login line present; no secrets.NUGET line
```

**Smoke (Actions, after merge to default branch)**
1. GitHub → Actions → **Release** → Run workflow → `mode=dry` → Run.
   - **Expect:** restore/build/pack succeed; **NuGet login** and **Push to NuGet** steps skipped.
2. Same, `mode=release`, leave `confirm` empty.
   - **Expect:** fails at **Validate break-glass confirmation**; no push.

**Automated gate**
- None in-repo (OIDC cannot be unit-tested locally).

**E2E (operator, next intended publish)**
1. Confirm NuGet.org TP policy: repo `TimeWarpEngineering/timewarp-multiavatar`, workflow **`release.yml`**, owner `TimeWarp.Enterprises`.
2. Publish a **GitHub Release** (tag = intended version; optional `v` prefix).
3. **Expect:** job has OIDC permissions; NuGet login succeeds; `dotnet nuget push` succeeds; package on nuget.org.
4. After green: delete repo secret `NUGET_API_KEY`; coordinate key revoke with nuru 458-009 if shared.

**Depends on:** merge of this `release.yml` onto the branch GitHub uses for `release` events (repo default).

**Not in scope:** full 458 reusable-workflow conversion; live E2E in the implementer session; org-wide key revocation before all secret-key publishers migrate.
