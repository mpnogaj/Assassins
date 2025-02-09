import prettier from 'eslint-plugin-prettier';
import typescriptEslint from '@typescript-eslint/eslint-plugin';
import globals from 'globals';
import tsParser from '@typescript-eslint/parser';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import js from '@eslint/js';
import { FlatCompat } from '@eslint/eslintrc';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
  baseDirectory: __dirname,
  recommendedConfig: js.configs.recommended,
  allConfig: js.configs.all
});

export default [
  {
    ignores: ['**/node_modules', '**/build', '**/dist', '**/public', '**/eslint.config.mjs']
  },
  ...compat.extends('eslint:recommended', 'plugin:@typescript-eslint/recommended'),
  {
    plugins: {
      prettier,
      '@typescript-eslint': typescriptEslint
    },

    rules: {
      // Disable the base rule as it can report incorrect errors
      'no-unused-vars': 'off',
      '@typescript-eslint/no-unused-vars': ['error', {
        varsIgnorePattern: '^_',
        argsIgnorePattern: '^_'
      }],

      'prettier/prettier': 'error'
    },

    languageOptions: {
      globals: {
        ...globals.node
      },

      parser: tsParser,
      ecmaVersion: 2020,
      sourceType: 'commonjs'
    }
  }
];