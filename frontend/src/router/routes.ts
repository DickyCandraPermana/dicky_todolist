import type { RouteRecordRaw } from "vue-router";
// Import komponen secara langsung atau lazy-load

export const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Home",
    component: () => import("@/pages/HomePage.vue"),
  },
  {
    path: "/login",
    name: "Login",
    component: () => import("@/pages/auth/LoginPage.vue"),
    // Tidak butuh meta requiresAuth karena ini halaman publik
  },
];
