#!/usr/bin/env node
import { existsSync, readFileSync } from 'node:fs';
import { resolve } from 'node:path';

const root = process.cwd();
const manifestPath = resolve(root, 'course/manifest.json');
const schemaPath = resolve(root, 'course/schema.json');

const requiredAgentSections = [
  'Task',
  'Context',
  'Allowed Files',
  'Blocked Files',
  'Spec Required',
  'Commands',
  'Acceptance',
  'Failure Handling',
  'Report Format',
];

const requiredHumanSections = [
  'Goal',
  'Run',
  'Observe',
  'Expected Visual State',
  'Key Files',
  'Walkthrough',
  'Common Failures',
  'Exercise',
  'Checkpoint',
  'Next',
];

const canonicalLessons = [
  {
    order: 0,
    id: '00-intro',
    title: 'Intro',
    kind: 'orientation',
    migrationSource: 'docs/tutorial/00-intro.md',
    route: '00-intro',
  },
  {
    order: 1,
    id: '01-setup',
    title: 'Setup',
    kind: 'setup',
    migrationSource: 'docs/tutorial/01-setup.md',
    route: '01-setup',
  },
  {
    order: 2,
    id: '02-first-window',
    title: 'First Window',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/02-first-window.md',
    route: '02-first-window',
  },
  {
    order: 3,
    id: '03-game-loop',
    title: 'Game Loop',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/03-game-loop.md',
    route: '03-game-loop',
  },
  {
    order: 4,
    id: '04-rendering',
    title: 'Rendering',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/04-rendering.md',
    route: '04-rendering',
  },
  {
    order: 5,
    id: '05-input',
    title: 'Input',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/05-input.md',
    route: '05-input',
  },
  {
    order: 6,
    id: '06-content-pipeline',
    title: 'Content Pipeline',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/06-content-pipeline.md',
    route: '06-content-pipeline',
  },
  {
    order: 7,
    id: '07-audio',
    title: 'Audio',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/07-audio.md',
    route: '07-audio',
  },
  {
    order: 8,
    id: '08-camera-collision-animation',
    title: 'Camera, Collision, And Animation',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/08-camera-collision-animation.md',
    route: '08-camera-collision-animation',
  },
  {
    order: 9,
    id: '09-publishing',
    title: 'Publishing',
    kind: 'experiment',
    migrationSource: 'docs/tutorial/09-publishing.md',
    route: '09-publishing',
  },
  {
    order: 10,
    id: '10-integrated-demo',
    title: 'Integrated Demo',
    kind: 'capstone',
    migrationSource: 'docs/tutorial/10-integrated-demo.md',
    route: '10-integrated-demo',
  },
];

let errors = 0;

function fail(message) {
  console.error(`ERROR: ${message}`);
  errors += 1;
}

function readJson(path) {
  try {
    return JSON.parse(readFileSync(path, 'utf8'));
  } catch (error) {
    fail(`${path} is not valid JSON: ${error.message}`);
    return null;
  }
}

function assertNonEmptyString(value, label) {
  if (typeof value !== 'string' || value.trim() === '') {
    fail(`${label} must be a non-empty string`);
  }
}

function assertArray(value, label) {
  if (!Array.isArray(value)) {
    fail(`${label} must be an array`);
    return [];
  }
  return value;
}

function assertNonEmptyArray(value, label) {
  const array = assertArray(value, label);
  if (array.length === 0) {
    fail(`${label} must not be empty`);
  }
  return array;
}

function assertPathExists(path, label) {
  if (typeof path !== 'string' || path.trim() === '') {
    fail(`${label} must be a non-empty path`);
    return;
  }
  if (!existsSync(resolve(root, path))) {
    fail(`${label} does not exist: ${path}`);
  }
}

function hasHeading(content, heading) {
  const escaped = heading.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  const pattern = new RegExp(`^#{1,6}\\s+${escaped}\\s*$`, 'm');
  return pattern.test(content);
}

function validateTopLevel(manifest) {
  if (manifest.version !== 1) {
    fail('version must be 1');
  }

  assertNonEmptyString(manifest.title, 'title');

  const tracks = assertArray(manifest.tracks, 'tracks');
  if (tracks.length !== 2 || tracks[0] !== 'human' || tracks[1] !== 'agent') {
    fail('tracks must be exactly ["human", "agent"]');
  }

  assertNonEmptyArray(manifest.lessons, 'lessons');
}

function validateSchemaFile() {
  const schema = readJson(schemaPath);
  if (!schema) return;

  assertNonEmptyString(schema.title, 'course/schema.json.title');

  const lesson = schema.$defs?.lesson;
  if (!lesson) {
    fail('course/schema.json.$defs.lesson is required');
    return;
  }

  const required = assertArray(lesson.required, 'course/schema.json.$defs.lesson.required');
  for (const field of ['migrationSource', 'route']) {
    if (!required.includes(field)) {
      fail(`course/schema.json lesson.required is missing ${field}`);
    }
  }

  const migrationSource = lesson.properties?.migrationSource;
  if (migrationSource?.type !== 'string' || migrationSource?.minLength !== 1) {
    fail('course/schema.json lesson.properties.migrationSource must be a non-empty string');
  }

  const route = lesson.properties?.route;
  if (route?.type !== 'string' || route?.pattern !== '^[0-9]{2}-[a-z0-9]+(?:-[a-z0-9]+)*$') {
    fail('course/schema.json lesson.properties.route must use the canonical slug pattern');
  }
}

function validateCanonicalLessons(manifest) {
  const actual = [...(manifest.lessons || [])].sort((a, b) => a.order - b.order);
  const actualIds = actual.map((lesson) => lesson.id);
  const expectedIds = canonicalLessons.map((lesson) => lesson.id);

  if (actual.length !== canonicalLessons.length) {
    fail(`manifest must contain exactly ${canonicalLessons.length} lessons; found ${actual.length}`);
  }

  for (let index = 0; index < canonicalLessons.length; index += 1) {
    const expected = canonicalLessons[index];
    const actualLesson = actual[index];
    if (!actualLesson) {
      fail(`missing canonical lesson at order ${expected.order}: ${expected.id}`);
      continue;
    }
    if (actualLesson.order !== expected.order || actualLesson.id !== expected.id) {
      fail(
        `canonical lesson mismatch at index ${index}: expected ${expected.order}/${expected.id}, found ${actualLesson.order}/${actualLesson.id}`,
      );
      continue;
    }
    if (actualLesson.title !== expected.title) {
      fail(`${expected.id}.title must be "${expected.title}"`);
    }
    if (actualLesson.kind !== expected.kind) {
      fail(`${expected.id}.kind must be ${expected.kind}`);
    }
    if (actualLesson.migrationSource !== expected.migrationSource) {
      fail(`${expected.id}.migrationSource must be ${expected.migrationSource}`);
    }
    if (actualLesson.route !== expected.route) {
      fail(`${expected.id}.route must be ${expected.route}`);
    }
  }

  for (const id of actualIds) {
    if (!expectedIds.includes(id)) {
      fail(`manifest contains non-v1 lesson id: ${id}`);
    }
  }
}

function validateSiteFiles() {
  assertPathExists('tutorial-site/src/pages/index.astro', 'tutorial site index route');
  assertPathExists('tutorial-site/src/pages/[...lesson].astro', 'tutorial site lesson route');
}

function validateReadmeTruth() {
  const readmePath = resolve(root, 'README.md');
  const content = existsSync(readmePath) ? readFileSync(readmePath, 'utf8') : '';

  if (!/Primary course entrypoint:\s*```bash\s*cd tutorial-site\s*npm install\s*npm run dev\s*```/ms.test(content)) {
    fail('README must route humans to the tutorial-site primary course entrypoint');
  }

  if (!content.includes('complete 00-10 v1 course from `course/manifest.json`')) {
    fail('README must state that the tutorial site renders the complete 00-10 v1 course from course/manifest.json');
  }

  if (!content.includes('Legacy migration source')) {
    fail('README must keep docs/tutorial documented as legacy migration source');
  }

  const stalePhrases = [
    'current manifest-backed course slice',
    'not a complete 00-10',
    'During migration',
    'intended canonical',
    'once the quality gate is implemented',
  ];
  for (const phrase of stalePhrases) {
    if (content.includes(phrase)) {
      fail(`README contains stale migration wording: ${phrase}`);
    }
  }
}

function validateLesson(lesson, seenIds, seenOrders, seenRoutes) {
  assertNonEmptyString(lesson.id, 'lesson.id');
  if (!/^[0-9]{2}-[a-z0-9]+(?:-[a-z0-9]+)*$/.test(lesson.id)) {
    fail(`lesson.id is not a valid slug: ${lesson.id}`);
  }

  if (seenIds.has(lesson.id)) {
    fail(`duplicate lesson id: ${lesson.id}`);
  }
  seenIds.add(lesson.id);

  if (!Number.isInteger(lesson.order)) {
    fail(`${lesson.id}.order must be an integer`);
  } else if (seenOrders.has(lesson.order)) {
    fail(`duplicate lesson order: ${lesson.order}`);
  }
  seenOrders.add(lesson.order);

  assertNonEmptyString(lesson.title, `${lesson.id}.title`);
  assertNonEmptyString(lesson.summary, `${lesson.id}.summary`);
  assertNonEmptyString(lesson.migrationSource, `${lesson.id}.migrationSource`);
  assertPathExists(lesson.migrationSource, `${lesson.id}.migrationSource`);
  assertNonEmptyString(lesson.route, `${lesson.id}.route`);

  if (lesson.route !== lesson.id) {
    fail(`${lesson.id}.route must match lesson id for v1`);
  }

  if (typeof lesson.route === 'string' && lesson.route.trim() !== '') {
    if (seenRoutes.has(lesson.route)) {
      fail(`duplicate lesson route: ${lesson.route}`);
    }
    seenRoutes.add(lesson.route);
  }

  const validKinds = new Set(['orientation', 'setup', 'experiment', 'capstone', 'appendix']);
  if (!validKinds.has(lesson.kind)) {
    fail(`${lesson.id}.kind is invalid: ${lesson.kind}`);
  }

  validateHumanTrack(lesson);
  validateAgentTrack(lesson);
  validateCode(lesson);
  validateCommands(lesson);
  validateEvidence(lesson);
}

function validateHumanTrack(lesson) {
  if (!lesson.human) {
    fail(`${lesson.id}.human is required`);
    return;
  }

  assertNonEmptyString(lesson.human.path, `${lesson.id}.human.path`);
  assertPathExists(lesson.human.path, `${lesson.id}.human.path`);

  const sections = assertNonEmptyArray(lesson.human.requiredSections, `${lesson.id}.human.requiredSections`);
  const contentPath = resolve(root, lesson.human.path);
  const content = existsSync(contentPath) ? readFileSync(contentPath, 'utf8') : '';

  for (const required of requiredHumanSections) {
    if (!sections.includes(required)) {
      fail(`${lesson.id}.human.requiredSections is missing required section: ${required}`);
    }
  }

  for (const section of sections) {
    assertNonEmptyString(section, `${lesson.id}.human.requiredSections[]`);
    if (!hasHeading(content, section)) {
      fail(`${lesson.human.path} is missing heading: ${section}`);
    }
  }
}

function validateAgentTrack(lesson) {
  if (!lesson.agent) {
    fail(`${lesson.id}.agent is required`);
    return;
  }

  assertNonEmptyString(lesson.agent.taskPath, `${lesson.id}.agent.taskPath`);
  assertPathExists(lesson.agent.taskPath, `${lesson.id}.agent.taskPath`);

  assertNonEmptyArray(lesson.agent.allowedFiles, `${lesson.id}.agent.allowedFiles`);
  assertArray(lesson.agent.blockedFiles, `${lesson.id}.agent.blockedFiles`);
  assertArray(lesson.agent.specRequiredFiles, `${lesson.id}.agent.specRequiredFiles`);

  for (const file of lesson.agent.allowedFiles) {
    if (!file.includes('*')) {
      assertPathExists(file, `${lesson.id}.agent.allowedFiles[]`);
    }
  }

  const taskPath = resolve(root, lesson.agent.taskPath);
  const content = existsSync(taskPath) ? readFileSync(taskPath, 'utf8') : '';
  for (const section of requiredAgentSections) {
    if (!hasHeading(content, section)) {
      fail(`${lesson.agent.taskPath} is missing heading: ${section}`);
    }
  }
}

function validateCode(lesson) {
  if (!lesson.code) {
    fail(`${lesson.id}.code is required`);
    return;
  }

  const projects = assertArray(lesson.code.projects, `${lesson.id}.code.projects`);
  const tests = assertArray(lesson.code.tests, `${lesson.id}.code.tests`);
  const keyFiles = assertNonEmptyArray(lesson.code.keyFiles, `${lesson.id}.code.keyFiles`);

  if ((lesson.kind === 'experiment' || lesson.kind === 'capstone') && projects.length === 0) {
    fail(`${lesson.id}.code.projects must not be empty for ${lesson.kind} lessons`);
  }

  for (const project of projects) assertPathExists(project, `${lesson.id}.code.projects[]`);
  for (const test of tests) assertPathExists(test, `${lesson.id}.code.tests[]`);
  for (const file of keyFiles) assertPathExists(file, `${lesson.id}.code.keyFiles[]`);
}

function validateCommands(lesson) {
  if (!lesson.commands) {
    fail(`${lesson.id}.commands is required`);
    return;
  }

  const run = assertArray(lesson.commands.run, `${lesson.id}.commands.run`);
  const verify = assertNonEmptyArray(lesson.commands.verify, `${lesson.id}.commands.verify`);

  if (!['orientation', 'appendix'].includes(lesson.kind) && run.length === 0) {
    fail(`${lesson.id}.commands.run must not be empty for ${lesson.kind} lessons`);
  }

  for (const command of run) assertNonEmptyString(command, `${lesson.id}.commands.run[]`);
  for (const command of verify) assertNonEmptyString(command, `${lesson.id}.commands.verify[]`);
}

function validateEvidence(lesson) {
  if (!lesson.evidence) {
    fail(`${lesson.id}.evidence is required`);
    return;
  }

  const validStatuses = new Set(['available', 'pending', 'notApplicable']);
  if (!validStatuses.has(lesson.evidence.status)) {
    fail(`${lesson.id}.evidence.status is invalid: ${lesson.evidence.status}`);
  }

  if (lesson.evidence.status !== 'available') {
    assertNonEmptyString(lesson.evidence.reason, `${lesson.id}.evidence.reason`);
  }

  const paths = assertArray(lesson.evidence.expectedPaths, `${lesson.id}.evidence.expectedPaths`);
  if (['available', 'pending'].includes(lesson.evidence.status) && paths.length === 0) {
    fail(`${lesson.id}.evidence.expectedPaths must not be empty when evidence is ${lesson.evidence.status}`);
  }

  if (lesson.evidence.status === 'available') {
    for (const path of paths) assertPathExists(path, `${lesson.id}.evidence.expectedPaths[]`);
  }
}

const manifest = readJson(manifestPath);
validateSchemaFile();
if (manifest) {
  validateTopLevel(manifest);
  validateCanonicalLessons(manifest);
  const seenIds = new Set();
  const seenOrders = new Set();
  const seenRoutes = new Set();
  for (const lesson of manifest.lessons || []) {
    validateLesson(lesson, seenIds, seenOrders, seenRoutes);
  }
}
validateSiteFiles();
validateReadmeTruth();

if (errors > 0) {
  console.error(`\n${errors} course validation error(s).`);
  process.exit(1);
}

console.log('Course manifest OK.');
