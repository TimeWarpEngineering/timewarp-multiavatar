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

## Checklist

- [ ] Produce and check in reference SVGs from the upstream generator, with provenance notes
- [ ] Jaribu test project under `tests/`, run by `bin/dev test`
- [ ] Golden equivalence tests
- [ ] Determinism, `sansEnv`, `ver` override tests
- [ ] Hash / part-selection known-value tests
- [ ] Embedded resource load test
- [ ] Edge input tests
- [ ] CI runs tests with coverage and uploads the artifact
- [ ] `ganda repo audit`: `ci-code-coverage` passes
- [ ] Boy-scout: remove the stale `LocalNuGetCache/` line from root `.gitignore` (leftover from archived task 005; the in-repo cache was removed in task 006)

## Session

- Created: b2020e9f-5c06-487a-a0b3-831ae1639f80 (2026-11-12)

## Notes

- Public surface today: `MultiavatarGenerator.Generate(string input, bool sansEnv = false,
  Dictionary<string, string>? ver = null)`, plus `HashService`, `PartTemplates`, `ThemeService`,
  `ColorReplacementService`.
- If a golden test shows a real difference from the web version, do not edit the reference to match.
  Record it in Results and file a bug task.
