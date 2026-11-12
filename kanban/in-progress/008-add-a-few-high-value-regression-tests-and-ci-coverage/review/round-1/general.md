# Round 1 — general

**Date:** 2026-11-12
**Scope reviewed:** branch `task/008-add-a-few-high-value-regression-tests-and-ci-cover` against merge-base `a6c26ce`. Product commit `f0fd9cf` (tests, references, CI coverage, `.gitignore`).

## Summary

The change adds a 24-case Jaribu suite and wires CI to collect Cobertura coverage and upload `artifacts/coverage`. The goal restatement matches the original task, including the boy-scout `.gitignore` item and the rule that a real upstream difference is filed rather than patched into a reference.

Checked against the repo and against `@multiavatar/multiavatar` 1.0.7 (npm dist shasum `bc2b855bab728a51ee9cee2412ee9f76282b7c78`, the value recorded in `references/readme.md`):

- Regenerating all 10 SVGs from `dist/commonjs/index.js` matches the checked-in files byte for byte. The unminified `multiavatar.js` matches that entry, including `''` for an empty input.
- The seven golden inputs plus the non-ASCII input use avatar types 00–15 and theme variants A, B, and C. SHA-256 digit hashes for those inputs match the known-value tests, including the `50` → `08B` half-up case.
- All 16 theme files and all 16 template files contain variants A/B/C and parts env, clo, head, mouth, eyes, top.
- The Release test assembly already in the worktree reports `total: 24`, `failed: 0`, with no `DEBUG:` lines under `--output Detailed`.
- `artifacts/coverage/coverage.cobertura.xml` names package `TimeWarp.Multiavatar` (line-rate about 0.80). The workflow uses the audit's coverage flags and an `actions/upload-artifact@v4` step named `coverage`. There is no percentage gate.

No findings.
