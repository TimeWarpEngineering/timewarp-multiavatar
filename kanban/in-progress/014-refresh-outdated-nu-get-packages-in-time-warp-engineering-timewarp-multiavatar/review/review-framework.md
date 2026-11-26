# Review framework — task 014

**Date:** 2026-11-26
**Host task:** kanban/in-progress/014-refresh-outdated-nu-get-packages-in-time-warp-engineering-timewarp-multiavatar/
**Diff scope:** branch `task/014-refresh-outdated-nu-get-packages-in-time-warp` vs `master` (merge-base `15974858137f47399d585278a8b897f82661cbdd`). Product files: `Directory.Packages.props`, `tools/dev-cli/dev.cs`, `tests/timewarp-multiavatar-tests/references/generate-references.cjs`.
**Plan / brief:** Bump the two dev-only Nuru pins (`TimeWarp.Nuru`, `TimeWarp.Nuru.DevCli`) from 4.0.0-beta.16 to 4.0.0-beta.17 in `Directory.Packages.props`. Do not change `<Version>` in `source/Directory.Build.props`. The implementer also turned on dev CLI telemetry and added a Purpose region so `ganda repo audit` passes.
**Effort:** 1 (general only)
**Reviewer roster:** general
**Session IDs:** review Grok `01a215f0-edae-7f10-b5da-a1d232956abb`; implementer Claude `7bcadc50-38f7-4603-a4c1-a948b693d57f`

## Budget (by-diff)

- Lines changed: 52
- Effort: 1
- TCB hits: none
- Roster axes: general
- Turn cap: 80 (--max-turns; cursor uncapped)

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims by reading the diff and the kitchen `## Results` and `### How to validate` record
- Do not re-run a full build, a full test run, an audit, or the hooks; those belong to the implementer's recorded results, the host audit node, and CI
- Exception: one targeted command may check one suspected bug; the finding names the command and the reason. A full build, full test run, or audit is never that command
- A missing or thin recorded result is a finding, not a reason to run the gates
- Prior rounds are immutable; new work goes in `round-(N+1)/`
