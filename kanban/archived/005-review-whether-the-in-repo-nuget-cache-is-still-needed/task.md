# Review whether the in-repo NuGet cache is still needed

## Description

Root `Directory.Build.props` (line 32) sets `<RestorePackagesPath>$(MSBuildThisFileDirectory)LocalNuGetCache</RestorePackagesPath>`, so every restored package lands inside the repo instead of `~/.nuget/packages`. `.gitignore` (line 23) ignores `LocalNuGetCache/`.

Context: the same scaffold leftover was just removed from `timewarp-build-tasks` (its task 006). Nothing there restored from a local feed, the folder was about 607 MB, and `ganda repo audit` checks that walk the disk without honoring .gitignore (`source-file-line-cap`, `analyzer-suppressions`) picked up restored package files and polluted the baselines.

The isolation only has a purpose when a repo restores its own locally built package from a local feed at the same version as a published one (it keeps those bits out of the shared cache).

## Requirements

- Determine whether anything in multiavatar (nuget.config sources, build scripts, samples, tests, CI) consumes a locally built package from a local feed.
- Check whether `ganda repo audit` baselines contain entries under `LocalNuGetCache/`.
- If not needed: remove the property, the folder and its .gitignore handling, re-seed any affected audit baselines, and verify build and pack.
- If needed: document why, and consider moving it under `artifacts/` (which those audit checks skip).

## Checklist

- [ ] Inventory local-feed consumers (nuget.config, build scripts, samples, tests, CI)
- [ ] Check audit baselines for `LocalNuGetCache/` entries
- [ ] Decide: remove or keep (and document)
- [ ] If removing: drop the property, the folder, and the .gitignore line; re-seed baselines
- [ ] If keeping: document why; consider moving under `artifacts/`
- [ ] Verify build and pack

## Session

- Created: ccd4893c-4c6d-47fb-9913-a5368de1e1b4 (2026-11-11)

## Notes

A quick config-only look found no `nuget.config` and no local feed configured. The only package-source reference is `--source https://api.nuget.org/v3/index.json` in `.github/workflows/workflow.yml` (line 130). No build scripts exist in the repo root. Samples and tests were not checked in depth.
