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

- [ ] Exactly one `.github/workflows/workflow.yml` remains, carrying all CI/CD (publish path included where the repo publishes)
- [ ] `sync-configurable-files.*` deleted (abandoned org mechanism)
- [ ] `*.disabled` / `*.bak` workflow cruft deleted
- [ ] Assistant workflows (claude*.yml), if present: explicitly kept (not CI/CD) or folded — record the call here
- [ ] CI still green after consolidation (and next publish verifies nuget/login where applicable)

## Notes

Created from timewarp-nuru 458-009/458 rollout session, 2026-08-08.
