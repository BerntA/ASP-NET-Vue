import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '', name: 'home', component: () => import('@/views/Home.vue') },
]

const router = createRouter({
  routes,
  history: createWebHistory(),
})

export default router
