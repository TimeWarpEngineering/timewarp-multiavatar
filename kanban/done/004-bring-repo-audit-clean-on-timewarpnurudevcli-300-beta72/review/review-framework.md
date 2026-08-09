# Review framework — task 004

**Date:** 2026-08-09
**Host task:** kanban/in-progress/004-bring-repo-audit-clean-on-timewarpnurudevcli-300-beta72/
**Diff scope:** commits `5fb6f7c..HEAD` (layout kebab + DevCli 3.0.0-beta.72 adoption); also compare vs plan in task Notes
**Plan / brief:** Bring repo audit-clean on TimeWarp.Nuru.DevCli 3.0.0-beta.72 — kebab product tree, Version SSOT in source/, adopt DevCli package (release/check-version/clean/self-install), remove LocalNuGetCache, pass `ganda repo audit`
**Effort:** 1 (general only)
**Reviewer roster:** general
**Session IDs:** orchestration grok 2026-08-09; implementer subagent 019fe6f1-c79f-7d20-abbd-0856dca7a1ab

## Ground rules

- Reviewers are read-only on product code; they write only under `review/round-N/`
- Severity: bug | suggestion | nit — Status starts as open
- Do not invent issues to fill space; zero issues is a valid outcome
- Address the diff and surrounding call sites; re-verify falsifiable claims against the repo
- Prior rounds are immutable; new work goes in `round-(N+1)/`
- Focus: correctness of layout migration (embedded resources), DevCli DI, Version SSOT, workflow Version path, audit-clean, no accidental API break
