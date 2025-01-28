import { defineConfig } from 'vite'
import plugin from '@vitejs/plugin-vue'
import path from 'path'

export default defineConfig({
  plugins: [plugin()],
  server: {
    port: 8080,
    proxy: {
      '/api': {
        target: 'https://localhost:7172',
        changeOrigin: true,
        secure: false,
      },
    }
  },
  resolve: {
    alias: [
      { find: '@', replacement: path.resolve(__dirname, 'src') },
    ],
  },
})
