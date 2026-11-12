# Upstream reference SVGs

Each `.svg` here is the exact output of the upstream JavaScript generator. The golden tests in
`multiavatar-generator.generate.cs` compare `MultiavatarGenerator.Generate` against these files
**byte for byte**. No normalisation is applied.

## Upstream version

- Package: `@multiavatar/multiavatar` **1.0.7** from npmjs.org (the package multiavatar.com links to)
- Entry point used: `dist/commonjs/index.js` (the package `main`)
- npm `dist.shasum`: `bc2b855bab728a51ee9cee2412ee9f76282b7c78`
- npm `gitHead`: `a9386febe5e992dce0d728269a22b2b74db8d979`
- Produced 2026-11-12 with Node v26.12.1
- The unminified `multiavatar.js` in the same package gives identical output for every file.

The live API at `api.multiavatar.com` sits behind a Cloudflare browser challenge, so it was not
used.

## Files

| File | Upstream call |
|------|---------------|
| `binx-bond.svg` | `multiavatar('Binx Bond')` |
| `starcrasher.svg` | `multiavatar('Starcrasher')` |
| `leia.svg` | `multiavatar('Leia')` |
| `alice.svg` | `multiavatar('alice')` |
| `bob.svg` | `multiavatar('bob')` |
| `digit-one.svg` | `multiavatar('1')` |
| `timewarp-multiavatar.svg` | `multiavatar('timewarp-multiavatar')` |
| `non-ascii.svg` | `multiavatar('Ünïcødé 漢字 🙂')` |
| `binx-bond.sans-env.svg` | `multiavatar('Binx Bond', true)` |
| `binx-bond.ver-11c.svg` | `multiavatar('Binx Bond', false, { part: '11', theme: 'C' })` |

Upstream returns `''` for an empty input; the test asserts that directly.

Each avatar takes its six parts from several avatar types. Between them these inputs use parts from
all 16 avatar types and all three theme variants (A, B, C).

## Regenerate

```bash
mkdir -p /tmp/multiavatar-upstream && cd /tmp/multiavatar-upstream
npm pack @multiavatar/multiavatar@1.0.7
tar xzf multiavatar-multiavatar-1.0.7.tgz
node <repo>/tests/timewarp-multiavatar-tests/references/generate-references.cjs \
  /tmp/multiavatar-upstream/package/dist/commonjs/index.js \
  <repo>/tests/timewarp-multiavatar-tests/references
```

If a golden test fails, the C# port differs from upstream. Do not edit a reference to make it pass.
Record the difference and file a bug task.
