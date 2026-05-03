import { readFileSync } from 'node:fs';

export interface CourseManifest {
  version: number;
  title: string;
  tracks: ['human', 'agent'];
  lessons: CourseLesson[];
}

export interface CourseLesson {
  id: string;
  order: number;
  title: string;
  summary: string;
  kind: 'orientation' | 'setup' | 'experiment' | 'capstone' | 'appendix';
  migrationSource: string;
  route: string;
  human: {
    path: string;
    requiredSections: string[];
  };
  agent: {
    taskPath: string;
    allowedFiles: string[];
    blockedFiles: string[];
    specRequiredFiles: string[];
  };
  code: {
    projects: string[];
    tests: string[];
    keyFiles: string[];
  };
  commands: {
    run: string[];
    verify: string[];
  };
  evidence: {
    status: 'available' | 'pending' | 'notApplicable';
    reason: string;
    expectedPaths: string[];
  };
}

const manifestUrl = new URL('../../../course/manifest.json', import.meta.url);

export function loadCourse(): CourseManifest {
  const raw = readFileSync(manifestUrl, 'utf8');
  const manifest = JSON.parse(raw) as CourseManifest;
  return {
    ...manifest,
    lessons: [...manifest.lessons].sort((a, b) => a.order - b.order),
  };
}

export function findLesson(id: string): CourseLesson | undefined {
  return loadCourse().lessons.find((lesson) => lesson.id === id);
}
