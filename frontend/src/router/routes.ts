import type { RouteRecordRaw } from "vue-router";
// Import komponen secara langsung atau lazy-load

export const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    component: () => import("@/layouts/MainLayout.vue"),
    children: [
      {
        path: "",
        name: "Home",
        component: () => import("@/pages/HomePage.vue"),
      },
      {
        path: "profile",
        name: "Profile",
        component: () => import("@/pages/profile/ProfilePage.vue"),
        meta: { requiresAuth: true },
      },
      {
        path: "todos",
        name: "Todos",
        component: () => import("@/pages/profile/todos/TodosPage.vue"),
        meta: { requiresAuth: true },
      },
      {
        path: "todos/create",
        name: "CreateTodo",
        component: () => import("@/pages/profile/todos/FormTodoPage.vue"),
        meta: { requiresAuth: true },
      },
      {
        path: "todos/:id/edit",
        name: "EditTodo",
        component: () => import("@/pages/profile/todos/FormTodoPage.vue"),
        meta: { requiresAuth: true },
      },
    ],
  },
  {
    path: "/",
    component: () => import("@/layouts/AuthLayout.vue"),
    children: [
      {
        path: "login",
        name: "Login",
        component: () => import("@/pages/auth/LoginPage.vue"),
        meta: { guestOnly: true },
      },
      {
        path: "register",
        name: "Register",
        component: () => import("@/pages/auth/RegisterPage.vue"),
        meta: { guestOnly: true },
      },
    ],
  },
];
