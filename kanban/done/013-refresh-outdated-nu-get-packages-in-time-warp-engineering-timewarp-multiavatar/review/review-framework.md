# Review framework — task 013

**Date:** 2026-11-19
**Host task:** kanban/in-progress/013-refresh-outdated-nu-get-packages-in-time-warp-engineering-timewarp-multiavatar/
**Diff scope:** branch `task/013-refresh-outdated-nu-get-packages-in-time-warp` vs `master` (merge-base). Product files: `Directory.Packages.props`, `tools/dev-cli/endpoints/workflow-command.cs`. Kitchen `task.md` is also on the branch.
**Plan / brief:** Bump the six outdated dev-only TimeWarp pins listed in the task. Leave `<Version>` in `source/Directory.Build.props` unchanged. The implementer also replaced the workflow self-install `Shell.Builder("dotnet")` call so Amuru 3.0.0-beta.10 analyzer AMURU0001 accepts the dev CLI.
**Effort:** 1 (general only)
**Reviewer roster:** general
**Session IDs:** review oracle Grok session `01a1f05c-986b-7e21-89ab-d78a004c937c` (2026-11-19). Implementer session was not reported (claude, 2026-11-18).

## Budget (by-diff)

- Lines changed: 75
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
