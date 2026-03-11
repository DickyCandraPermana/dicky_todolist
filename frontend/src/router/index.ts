import { createRouter, createWebHistory } from "vue-router";
import { routes } from "./routes"; // Impor rute dari file terpisah
import { setupGuards } from "./guards";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes, // Gunakan variabel routes di sini
  scrollBehavior(_to, _from, savedPosition) {
    return savedPosition || { top: 0 };
  },
});

// Anda juga bisa menambahkan Middleware (Navigation Guards) di sini
setupGuards(router);

export default router;
