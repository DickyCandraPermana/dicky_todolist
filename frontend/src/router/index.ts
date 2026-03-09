import { createRouter, createWebHistory } from "vue-router";
import { routes } from "./routes"; // Impor rute dari file terpisah
import { useAuthStore } from "@/stores/authStore";
// import { authStore } from "@/stores/authStore";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes, // Gunakan variabel routes di sini
  scrollBehavior(_to, _from, savedPosition) {
    return savedPosition || { top: 0 };
  },
});

// Anda juga bisa menambahkan Middleware (Navigation Guards) di sini
router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore();
  console.log(`Navigasi ke: ${to.name as string};`);
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({ name: "Login" }); // Redirect ke Login
  } else {
    next(); // Lanjutkan navigasi
  }
});

export default router;
