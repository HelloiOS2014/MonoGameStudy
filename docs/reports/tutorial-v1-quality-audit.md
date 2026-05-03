# Tutorial V1 Quality Audit

Date: 2026-05-03

## Objective

Complete the MonoGame dual-track tutorial v1 with content quality and mainline alignment both at or above 95/100.

## Prompt-To-Artifact Checklist

| Requirement | Evidence | Status |
| --- | --- | --- |
| `course/` is canonical | `course/manifest.json`; `README.md`; `AGENTS.md`; tutorial site loader | Pass |
| 00-10 human lessons exist | `course/lessons/00-intro.mdx` through `course/lessons/10-integrated-demo.mdx` | Pass |
| 00-10 agent packets exist | `course/agent-tasks/00-intro.md` through `course/agent-tasks/10-integrated-demo.md` | Pass |
| Tutorial site renders all lessons | `npm run build` output lists `/` plus 11 lesson pages | Pass |
| Tutorial site deploys by tag | `.github/workflows/pages.yml` builds `tutorial-site/` and deploys Pages on `v*` tags | Pass |
| Verifier enforces canonical coverage | `tools/check-course.mjs`; `./tools/check-course.sh` output | Pass |
| Runtime checks pass | `dotnet build GameDemo.sln -m:1`; `./tools/check-tutorial.sh` | Pass |
| No Godot or game expansion | Diff inspection; manifest mappings remain MonoGame research/course scope | Pass |

## Verification Evidence

| Command | Exit | Reading |
| --- | ---: | --- |
| `./tools/check-course.sh` | 0 | `Course manifest OK.` |
| `bash -n tools/check-env.sh` | 0 | Shell syntax accepted. |
| `bash -n tools/check-tutorial.sh` | 0 | Shell syntax accepted. |
| `bash -n tools/check-course.sh` | 0 | Shell syntax accepted. |
| `git diff --check` | 0 | No whitespace errors. |
| `cd tutorial-site && npm run build` | 0 | Astro check reported 0 errors, 0 warnings, 0 hints; build emitted 12 pages. |
| `GITHUB_ACTIONS=true GITHUB_REPOSITORY=HelloiOS2014/MonoGameStudy npm run build` | 0 | Simulated Pages base path build emitted 12 pages with `/MonoGameStudy/` links. |
| `dotnet build GameDemo.sln -m:1` | 0 | 0 warnings, 0 errors. |
| `./tools/check-tutorial.sh` | 0 | `Tutorial dry run passed.` |

## Environment Note

The first sandboxed `./tools/check-tutorial.sh` run failed because MonoGame GUI smoke apps could not create an OpenGL graphics device in the sandbox, and an e10 publish restore hit sandbox DNS limits. The final escalated run passed the full dry run, including GUI smoke windows, deliberate MGCB failure, e10 publish, published executable smoke, and integrated demo smoke.

## Hard Cap Check

No content quality cap applies.

No mainline alignment cap applies.

## Content Quality Score

| Category | Max | Score | Evidence |
| --- | ---: | ---: | --- |
| Canonical lesson completeness | 20 | 20 | 11 lessons, manifest entries, routes, evidence paths. |
| Human learning usefulness | 20 | 19 | Every lesson has run, observe, expected visual state, walkthrough, exercise, checkpoint, and next step. |
| Visual expectation clarity | 15 | 15 | Expected Visual State sections and matching evidence files cover every lesson. |
| Code mapping quality | 15 | 15 | Projects, tests, commands, and key files resolve through `check-course`. |
| Agent task usefulness | 15 | 14 | Packets are lesson-scoped, narrow allowed files, and include verification/reporting. |
| Site usability | 10 | 10 | Manifest-driven Astro site builds all 11 lesson routes plus index and has tag-driven Pages deployment. |
| Entry point honesty | 5 | 5 | README, AGENTS, legacy docs, and roadmap route users to current v1 sources. |
| Total | 100 | 98 | Evidence-backed. |

## Mainline Alignment Score

| Category | Max | Score | Evidence |
| --- | ---: | ---: | --- |
| `course/` source-of-truth discipline | 25 | 25 | Manifest drives lessons, tasks, evidence paths, and site routing. |
| MonoGame research continuity | 20 | 20 | Lessons map to existing experiments, integrated demo, tools, and reports. |
| Scope control | 15 | 15 | No Godot, no new experiment, no integrated demo expansion. |
| Human/agent separation | 15 | 14 | Human lessons and agent packets are separate and cross-referenced through manifest. |
| Verifier coverage discipline | 15 | 15 | `check-course` enforces canonical coverage, entrypoint truth, required sections, and path existence. |
| Documentation consistency | 10 | 10 | README, AGENTS, legacy docs, roadmap, and audit align. |
| Total | 100 | 99 | Evidence-backed. |

## Counterargument Review

- Passing `check-course` does not prove lesson prose quality; lesson prose was reviewed against every required section, expected state, exercise, and evidence file.
- Passing `npm run build` does not prove runtime behavior; `dotnet build` and `check-tutorial` provide runtime evidence.
- Textual expected visual state is weaker than screenshots; screenshots are not required for v1, and every GUI lesson has expected visual state plus smoke command evidence.
- Agent packets do not prove every future agent will behave correctly; they reduce prompt size and scope ambiguity by making allowed, blocked, and spec-required files explicit.

## Result

Content quality: 98/100.

Mainline alignment: 99/100.

V1 quality gate passes.
