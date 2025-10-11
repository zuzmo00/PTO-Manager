import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:49865', // <-- backend címe (pl. Express vagy Nest)
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
