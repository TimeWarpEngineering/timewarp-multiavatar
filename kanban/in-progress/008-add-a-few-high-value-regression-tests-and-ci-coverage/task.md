# Add a few high-value regression tests and CI coverage

## Description

The library has no tests. It has been manually tested a lot and is meant to be equivalent to the web
version at https://multiavatar.com/ (the upstream JavaScript `multiavatar` generator).

Add a **small** suite that guards against regression. Steve 2026-11-12: "we do not need a ton of test to
make this slow but some to make sure we don't regress later."

This also clears the `ci-code-coverage` audit failure, which is skipped today only because there is no
test project.

## Requirements

### Tests (keep it small and fast)

A handful of tests, not exhaustive coverage. The whole suite should run in a few seconds. Suggested set:

- **Golden equivalence with the web version.** For a short fixed list of inputs (about 5 to 8, chosen to
  cover different avatar types and theme variants A/B/C), `MultiavatarGenerator.Generate(input)` equals a
  checked-in reference SVG produced by the upstream generator (multiavatar.com / the upstream
  `multiavatar` JS). Record in the test folder how each reference was produced and from which upstream
  version. If byte-for-byte equality is not achievable, state the exact normalisation used and why.
- **Determinism.** The same input gives the same SVG on repeated calls.
- **`sansEnv`.** `Generate(input, sansEnv: true)` omits the environment part and otherwise matches.
- **`ver` override.** `Generate(input, ver: …)` forces the given part and theme.
- **Hash to part selection.** One or two known-value checks on `HashService.GenerateHash` /
  `GetPartSelection`, since every avatar depends on them.
- **Embedded resources load.** Every avatar type's parts and every theme resolve (cheap loop over
  `PartTemplates` / `ThemeService`). This is what would break silently on a folder or resource rename, as
  nearly happened in task 006.
- **Edge input.** Empty string and a non-ASCII input behave the same as upstream.

Do not add a test per avatar or per theme combination.

### Framework and layout

- Use Jaribu (see the `tw-jaribu` skill) under `tests/`, wired into `bin/dev test`.
- The library writes debug traces to stderr through `TimeWarp.Terminal`. Tests must not depend on that
  output, and it should not flood the test log if that is easy to avoid.

### CI coverage (`ci-code-coverage`)

- CI runs the tests and collects coverage, per the audit's own instructions: the Microsoft Testing
  Platform invocation gets
  `--results-directory artifacts/coverage --coverage --coverage-output-format cobertura --coverage-output coverage.cobertura.xml`,
  followed by an `actions/upload-artifact@v4` step for `artifacts/coverage`.
- Try `ganda repo audit --fix` first once a test project exists; it may wire this.
- No coverage percentage gate.

### Gates

- `bin/dev build` 0 warnings / 0 errors, `bin/dev test` green, `ganda repo audit` with `ci-code-coverage`
  passing, CI green on the PR.

## Goal (implementer restatement)

Add a small, fast regression suite so later changes cannot silently break the C# port of the upstream
`multiavatar` JavaScript generator, and make CI run it with coverage.

Acceptance:

- A Jaribu (Microsoft Testing Platform) project under `tests/` that `bin/dev test` and `dotnet test` run,
  finishing in a few seconds.
- Golden tests: 5 to 8 inputs spanning avatar types and theme variants A/B/C equal checked-in SVGs made
  by the upstream generator, with provenance (upstream version, how produced) recorded beside them.
- Determinism, `sansEnv`, and `ver` override tests; known-value tests for `HashService`; a loop proving
  every embedded template and theme resolves; empty and non-ASCII input match upstream.
- The library's stderr debug traces do not reach the test log, and no test depends on them.
- CI passes the MTP coverage flags and uploads `artifacts/coverage`; no percentage gate.
- `bin/dev build` 0/0, `bin/dev test` green, `ganda repo audit` clean (including `ci-code-coverage`).
- Remove the stale `LocalNuGetCache/` line from `.gitignore`.
- A real difference from upstream is recorded and filed, never "fixed" by editing a reference.

## Checklist

- [x] Produce and check in reference SVGs from the upstream generator, with provenance notes
- [x] Jaribu test project under `tests/`, run by `bin/dev test`
- [x] Golden equivalence tests
- [x] Determinism, `sansEnv`, `ver` override tests
- [x] Hash / part-selection known-value tests
- [x] Embedded resource load test
- [x] Edge input tests
- [x] CI runs tests with coverage and uploads the artifact
- [x] `ganda repo audit`: `ci-code-coverage` passes
- [x] Boy-scout: remove the stale `LocalNuGetCache/` line from root `.gitignore` (leftover from archived task 005; the in-repo cache was removed in task 006)

## Session

- Created: b2020e9f-5c06-487a-a0b3-831ae1639f80 (2026-11-12)
- Implement (ganda task work, 2026-11-12): references from npm `@multiavatar/multiavatar` 1.0.7; Jaribu
  MTP project; CI coverage; audit clean. Filed backlog task 010 for the dev-cli build break.
- Implement oracle: implemented by implementer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session d1292e96-2a93-410a-8211-97c75d39209d, max-turns 500, Done — 2026-11-12T10:18:44Z

## Notes

- Public surface today: `MultiavatarGenerator.Generate(string input, bool sansEnv = false,
  Dictionary<string, string>? ver = null)`, plus `HashService`, `PartTemplates`, `ThemeService`,
  `ColorReplacementService`.
- If a golden test shows a real difference from the web version, do not edit the reference to match.
  Record it in Results and file a bug task.

## Results

### What changed

- `tests/timewarp-multiavatar-tests/` — Jaribu on Microsoft Testing Platform (`TimeWarp.Jaribu.TestingPlatform`
  1.0.0-beta.15, Shouldly 4.3.0, `Microsoft.Testing.Extensions.CodeCoverage` 18.12.0). Added to
  `timewarp-multiavatar.slnx`, so `bin/dev test` / `dotnet test` run it. 24 test cases, about 0.3 s.
  - `multiavatar-generator.generate.cs`: 7 golden inputs, non-ASCII golden, empty input, determinism,
    `sansEnv` (matches upstream and equals the full SVG minus the env segment), `ver` override.
  - `hash-service.generate-hash.cs`, `hash-service.get-part-selection.cs`: upstream known values, including
    `"50"`, the one slice that lands exactly on .5 (JS `Math.round` vs .NET banker's rounding agree there).
  - `embedded-resources.load.cs`: all 16 types x 6 parts resolve, all 16 themes x A/B/C x 6 parts have colours.
  - `quiet-terminal.cs`: a module initializer sets `TimeWarp.Terminal.Terminal.Instance` to a `TestTerminal`,
    so the library's DEBUG lines never reach the test log (0 lines in `--output Detailed`).
- `references/`: 10 SVGs from upstream 1.0.7 plus `readme.md` (version, checksums, call per file,
  regenerate steps) and `generate-references.cjs`. **Byte-for-byte equality, no normalisation.** The
  10 references between them use all 16 avatar types and all three variants.
- No difference from upstream found, so no bug task was needed.
- Test classes are `internal abstract` in namespace `TimeWarp.Multiavatar.Tests`, with
  `Sut_Action_Given_` class names. That keeps CA1707/CA1812/CA1515 quiet without any new suppression
  (`analyzer-suppressions` baseline unchanged).
- Root `global.json` sets `"test": { "runner": "Microsoft.Testing.Platform" }` (no SDK pin).
- CI (`.github/workflows/workflow.yml`): the test step passes
  `--results-directory artifacts/coverage --coverage --coverage-output-format cobertura --coverage-output coverage.cobertura.xml`;
  a new `coverage` step uploads `artifacts/coverage` with `actions/upload-artifact@v4`. No percentage gate.
  Wired by hand from the audit's own instructions; the check is not auto-fixable here.
- `.gitignore`: removed the stale `LocalNuGetCache/` line.

### Audit (boy-scout)

- `ci-code-coverage`: FAIL -> PASS.
- `vscode-window-icon`: fixed by `ganda repo audit --fix` (writes the git-ignored `.vscode/` files; nothing to commit).
- `bin-dev` / `dev-cli-capabilities`: the dev CLI no longer builds from source since task 009
  (TimeWarp.SourceGenerators 1.0.0-beta.14 TWA0038 fires on two >500-line files shipped as
  TimeWarp.Nuru.DevCli 4.0.0-beta.12 content). Not fixable in this repo without a suppression. The
  dev-cli source is unchanged since master's `bin/dev` was published, so that binary was copied into this
  worktree's git-ignored `bin/`. Follow-up: backlog task **010** (blocked on timewarp-nuru 576/577).
- Final `ganda repo audit`: 43 passed, 0 failed, 3 skipped.

### Gates run

- `./bin/dev build`: 0 warnings, 0 errors.
- `./bin/dev test`: 24/24 passed.
- `dotnet test -c Release --no-build` with the CI coverage flags: 24/24 passed; writes
  `artifacts/coverage/coverage.cobertura.xml` covering `TimeWarp.Multiavatar` (about 80 % lines).
- Mutation check: altering one byte of `leia.svg` in the output folder fails exactly that golden case.
- `ganda repo audit`: clean.

### How to validate

Smoke:

```bash
./bin/dev build
./bin/dev test
dotnet test -c Release --no-build --results-directory artifacts/coverage --coverage --coverage-output-format cobertura --coverage-output coverage.cobertura.xml
ganda repo audit
```

Expect:

- Build: 0 warnings, 0 errors.
- Tests: `total: 24, failed: 0`, in well under 5 seconds, with no `DEBUG:` lines in the output.
- `artifacts/coverage/coverage.cobertura.xml` exists and lists package `TimeWarp.Multiavatar`.
- Audit: `ci-code-coverage` PASS, no failures (a fresh worktree needs `bin/dev`; see task 010).
- On the PR, CI shows a `coverage` artifact.

