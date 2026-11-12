// Regenerates the reference SVGs in this folder from the upstream JavaScript generator.
// See readme.md in this folder for the exact steps and upstream version.
//
// usage: node generate-references.cjs <path-to-upstream dist/commonjs/index.js> <output-dir>
const fs = require('fs');
const path = require('path');

const multiavatar = require(process.argv[2]);
const out = process.argv[3];
fs.mkdirSync(out, { recursive: true });

const golden = {
  'binx-bond': 'Binx Bond',
  'starcrasher': 'Starcrasher',
  'leia': 'Leia',
  'alice': 'alice',
  'bob': 'bob',
  'digit-one': '1',
  'timewarp-multiavatar': 'timewarp-multiavatar',
  'non-ascii': 'Ünïcødé 漢字 🙂',
};

for (const [name, input] of Object.entries(golden)) {
  fs.writeFileSync(path.join(out, name + '.svg'), multiavatar(input));
}

fs.writeFileSync(path.join(out, 'binx-bond.sans-env.svg'), multiavatar('Binx Bond', true));
fs.writeFileSync(path.join(out, 'binx-bond.ver-11c.svg'), multiavatar('Binx Bond', false, { part: '11', theme: 'C' }));

// Upstream returns '' for an empty input; the test asserts that directly, so no file is written.
if (multiavatar('') !== '') throw new Error('upstream no longer returns an empty string for empty input');
