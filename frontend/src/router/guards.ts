// src/router/guards.ts
import { useAuthStore } from "@/stores/authStore";

export const setupGuards = (router: any) => {
  router.beforeEach(async (to: any, _from: any) => {
    const authStore = useAuthStore();

    // Jika halaman butuh auth (meta: { requiresAuth: true })
    if (to.meta.requiresAuth && !authStore.isAuthenticated) {
      return "/login";
    }

    if (to.meta.guestOnly && authStore.isAuthenticated) {
      return "/todos";
    }

    return true;
  });
};
