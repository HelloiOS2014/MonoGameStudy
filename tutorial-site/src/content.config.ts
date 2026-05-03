import { defineCollection, z } from 'astro:content';
import { glob } from 'astro/loaders';

const lessons = defineCollection({
  loader: glob({ pattern: '*.mdx', base: '../course/lessons' }),
  schema: z.object({
    id: z.string(),
    title: z.string(),
  }),
});

export const collections = { lessons };
