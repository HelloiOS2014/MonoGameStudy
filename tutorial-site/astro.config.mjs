import { defineConfig } from 'astro/config';
import mdx from '@astrojs/mdx';

const isGitHubActions = process.env.GITHUB_ACTIONS === 'true';
const repositoryName = process.env.GITHUB_REPOSITORY?.split('/')[1] ?? 'MonoGameStudy';

export default defineConfig({
  base: isGitHubActions ? `/${repositoryName}/` : '/',
  integrations: [mdx()],
});
