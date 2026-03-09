// src/router/guards.ts
import { useAuthStore } from "@/stores/authStore";

export const setupGuards = (router: any) => {
  router.beforeEach(async (to: any, _from: any, next: any) => {
    const authStore = useAuthStore();

    // Jika halaman butuh auth (meta: { requiresAuth: true })
    if (to.meta.requiresAuth && !authStore.isAuthenticated) {
      next("/login");
    }
    // Jika sudah login tapi mau ke halaman login lagi
    else if (to.path === "/login" && authStore.isAuthenticated) {
      next("/todos");
    } else {
      next("/login");
    }
  });
};
