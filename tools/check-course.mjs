#!/usr/bin/env node
import { existsSync, readFileSync } from 'node:fs';
import { resolve } from 'node:path';

const root = process.cwd();
const manifestPath = resolve(root, 'course/manifest.json');

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

function validateLesson(lesson, seenIds, seenOrders) {
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
if (manifest) {
  validateTopLevel(manifest);
  const seenIds = new Set();
  const seenOrders = new Set();
  for (const lesson of manifest.lessons || []) {
    validateLesson(lesson, seenIds, seenOrders);
  }
}

if (errors > 0) {
  console.error(`\n${errors} course validation error(s).`);
  process.exit(1);
}

console.log('Course manifest OK.');
