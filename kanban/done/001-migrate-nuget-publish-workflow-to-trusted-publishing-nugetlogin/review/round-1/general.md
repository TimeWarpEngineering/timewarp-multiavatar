# Round 1 — general
**Date:** 2026-08-08
**Scope reviewed:** .github/workflows/release.yml (commit 338e80a)

## Summary

The migration matches the locked plan and checklist.

**Verified against repo**

| Requirement | Result |
|-------------|--------|
| Job `permissions`: `contents: read` + `id-token: write` | Present on `release` job (lines 23–26) |
| `nuget/login@v1` user `TimeWarp.Enterprises` | Present (lines 68–73) |
| Login gated on publish condition | Same condition as push (release event OR dispatch `mode=release` + `confirm=release`) |
| No `secrets.NUGET_API_KEY` | No secret-key refs under `.github/`; push uses `steps.nuget-login.outputs.NUGET_API_KEY` only |
| Filename kept as `release.yml` | Unchanged (TP policy name-sensitive) |
| Primary trigger `release: types: [published]` | Present; any-tag push path removed |
| Break-glass: validate `mode=release` without `confirm=release` | Fail-fast step (lines 37–40) |
| Version: `release.tag_name` or root `Directory.Build.props` | Present; root props has `<Version>1.0.0-beta.13</Version>` |

**Control flow notes (no defects found)**

- Publish `if:` short-circuits on `github.event_name == 'release'`, so `inputs.*` are not required for release events.
- Dry `workflow_dispatch` builds/packs and skips login + push.
- Login and push share the same gate, so push cannot run without a successful login step when publishing.

Residual operator work remains outside this diff (task checklist): E2E publish on next release, then revoke long-lived NuGet key / delete GitHub secret.

## Issues

None.
