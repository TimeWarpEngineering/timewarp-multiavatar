# Adopt the shared pull request build classifier and the one-workflow CI standard

## Description

Bring timewarp-multiavatar to the org CI standard (ganda 476 / 479 / 565 / 587, audited by ganda 655): `workflow.yml` has
no `paths` / `paths-ignore` filter on `pull_request`, so every pull request reports `CI`, and `dev workflow`
calls the shared classifier `PullRequestBuildClassifier` from `TimeWarp.Nuru.DevCli` 4.0.0-beta.15
(timewarp-nuru 595) to skip build and test on a pull request that touches no build path (kanban, docs, ...).

Tracked by timewarp-ganda 656 (architecture reposet rollout).

Survey 2026-11-16 for this repo: no `pull_request` paths filter; `dev workflow` has a local copy of the classification rule. Work: switch to the package classifier and delete the local copy.

## Requirements

1. Run `ganda repo audit --fix` first and commit what it changes.
2. Bump the `TimeWarp.Nuru.DevCli` pin to `4.0.0-beta.15` (and `TimeWarp.Nuru` to the same version if pinned).
3. In the `workflow` command's pull-request path, call `PullRequestBuildClassifier.DecideAsync` first. On skip,
   print its message and exit 0. Push and release paths are unchanged.
4. Remove `paths` / `paths-ignore` from the `pull_request` trigger of `.github/workflows/workflow.yml`. Keep any
   `push` filter and add a comment saying why (a kanban-only merge must not rebuild and upload packages).
5. If this repo builds from roots outside the classifier defaults (`source`, `tests`, `tools`, `msbuild`), declare
   them through the classifier's config (`PullRequestBuildConfig`), not by copying or editing the rule.
6. No local copy of the classification rule remains in `tools/dev-cli/`.
7. `ganda repo audit` passes, including both CI-standard checks from ganda 655.

## Goal (implementer restatement)

Make every pull request to timewarp-multiavatar report a `CI` check, and let `dev workflow` decide
whether that run builds, using the org's single shared rule instead of a hand copy.

Acceptance:
- `ganda repo audit --fix` runs first and its changes are committed.
- `TimeWarp.Nuru` and `TimeWarp.Nuru.DevCli` are pinned at `4.0.0-beta.15`.
- `dev workflow` calls `PullRequestBuildClassifier.DecideAsync(repoRoot, ct)` first; when it says skip,
  it prints the decision message and exits 0. Push/release/local runs keep the full pipeline.
- The local `NonBuildPaths` copy in `tools/dev-cli/endpoints/workflow-command.cs` is deleted.
- `.github/workflows/workflow.yml` has no `paths`/`paths-ignore` on `pull_request` (any `push`
  filter is kept with a why comment).
- Build roots beyond the classifier defaults are declared in `.timewarp/dev.jsonc`
  `pullRequestBuild` (readme.md is packed into the NuGet package, so it is a build path).
- `ganda repo audit` passes, including the two CI-standard checks (ganda 655).

## Checklist

- [x] `ganda repo audit --fix` run and committed
- [x] DevCli pin at 4.0.0-beta.15
- [x] `dev workflow` calls the shared classifier on pull requests; skip exits 0 with its message
- [x] No `paths` filter on `pull_request`; `push` filter kept with a why comment
- [x] Extra build roots (if any) declared through config
- [x] `ganda repo audit` clean
- [ ] This task's pull request shows a `CI` run (record the URL in Results)

## Notes


After merge, the ganda 656 tracker proves a kanban-only pull request here reports `CI` success and merges with
plain `ganda pr merge` (no `--allow-no-ci`).

## Session

- 2026-11-17 implementer (claude): `ganda repo audit --fix` added `.githooks/prepare-commit-msg{,.cs}`
  (committed f76fb36). The audit's `package-pin-currency` check also flagged TimeWarp.Amuru /
  Amuru.Tools 3.0.0-beta.2 (latest 3.0.0-beta.5); bumped them with the Nuru pins (boy-scout).

- Created: 64677b22-d705-4b25-b537-4b53732bcb92 (2026-11-16)
- Implement oracle: implemented by implementer-claude (claude, vendor anthropic, model claude-opus-5-5, response model claude-opus-5-5), session 371cd5eb-9021-40a8-b8e6-c821d35d1272, max-turns 500, Done — 2026-11-16T18:58:44Z

## Results

Changes:
- `Directory.Packages.props`: `TimeWarp.Nuru` and `TimeWarp.Nuru.DevCli` 4.0.0-beta.13 -> 4.0.0-beta.15;
  `TimeWarp.Amuru` and `TimeWarp.Amuru.Tools` 3.0.0-beta.2 -> 3.0.0-beta.5 (pin-currency audit).
- `tools/dev-cli/endpoints/workflow-command.cs`: `dev workflow` starts with
  `PullRequestBuildClassifier.DecideAsync(RepoRoot, Ct)`, prints `decision.Message`, and returns
  (exit 0) when `!decision.RequiresBuild`. The local `NonBuildPaths` rule copy and its diff/event
  helpers are deleted. Push/release/local runs: the classifier says "Full build" and the pipeline runs.
- `.timewarp/dev.jsonc` (new): `pullRequestBuild.buildPaths = ["readme.md", "assets/logo.png"]`,
  both packed into the nupkg. No other build roots outside the classifier defaults.
- `.github/workflows/workflow.yml`: `pull_request` had no `paths` filter and `push` had none to keep.
  Added a header comment on the one-workflow standard, `fetch-depth: 0` (the classifier's three-dot
  diff), and a pull-request step `dotnet run --file tools/dev-cli/dev.cs -- workflow`. Before this
  change CI never called `dev workflow`, so the classifier would have had no effect in CI. Push and
  dispatch-merge keep the dotnet restore/build/test/coverage steps. Release steps are unchanged.
  Pull requests no longer upload the coverage artifact; push to master still does.

### Gates

- `./bin/dev build`: succeeded, 0 warnings, 0 errors.
- `./bin/dev test`: Passed, 24/24.
- `ganda repo audit`: passes all checks (44 passed, 0 failed, 3 skipped), including
  `dev-workflow-classifier` and `workflow-file`.
- Dev CLI recompiled with `dotnet run --file tools/dev-cli/dev.cs -- self-install` (succeeded).

### How to validate

Smoke:
```bash
./bin/dev workflow   # local run, no GITHUB_EVENT_NAME
# Simulated kanban-only pull request (35b19f8...80f2261 changes only kanban/**):
printf '{"pull_request":{"base":{"sha":"%s"},"head":{"sha":"%s"}}}' \
  $(git rev-parse 35b19f8) $(git rev-parse 80f2261) > /tmp/pr.json
GITHUB_EVENT_NAME=pull_request GITHUB_EVENT_PATH=/tmp/pr.json ./bin/dev workflow; echo $?
```

Expect:
- Local run prints `No GITHUB_EVENT_NAME (local run). Full build.` then
  `Pipeline: self-install -> clean -> build -> test`.
- Simulated kanban-only pull request prints `Pull request changes only declared non-build paths.
  Skipping build and test. ... (2 changed paths)`, then `Workflow completed successfully!`, exit 0.
  (Verified 2026-11-17.) A diff with a build path prints `Build path changed: <path>. Full build.`
- This task's pull request shows a `ci` check from the `CI/CD` workflow. The URL is recorded after
  the host opens the pull request; it is not available during implement.

