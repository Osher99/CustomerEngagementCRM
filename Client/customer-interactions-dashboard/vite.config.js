import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import crypto from 'crypto-browserify';

export default defineConfig({
  plugins: [react()],
  define: {
    'process.env': {},
    crypto: crypto,
  },
  server: {
    port: 3000,
    strictPort: true,
  },
});
