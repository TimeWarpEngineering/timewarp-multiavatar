# Review framework — task 008

**Date:** 2026-11-12
**Host task:** kanban/in-progress/008-add-a-few-high-value-regression-tests-and-ci-coverage/
**Diff scope:** branch `task/008-add-a-few-high-value-regression-tests-and-ci-cover` against merge-base `a6c26ce` (master). Product commit `f0fd9cf`.
**Plan / brief:** Small Jaribu regression suite for the C# multiavatar port (golden SVGs from upstream 1.0.7, determinism, sansEnv, ver, hash, embedded resources, edge inputs) plus CI coverage upload. No percentage gate.
**Effort:** 2 (general only)
**Reviewer roster:** general
**Session IDs:** reviewer grok (grok-4.7), body tw-implementation-review, print mode. Session id not reported. Turn cap 120; cursor session uncapped (no `--max-turns` flag).

## Budget (by-diff)

- Lines changed: 669
- Effort: 2
- TCB hits: none
- Roster axes: general
- Turn cap: 120 (--max-turns; cursor uncapped)

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims against the repo
- Prior rounds are immutable; new work goes in `round-(N+1)/`
