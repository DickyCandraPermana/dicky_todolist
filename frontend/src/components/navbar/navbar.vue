<script setup lang="ts">
import { computed } from "vue";
import { RouterLink, useRoute } from "vue-router";
import { useAuthStore } from "@/stores/authStore";

const authStore = useAuthStore();
const route = useRoute();

const isHomeActive = computed(() => route.path === "/");
const isTodosActive = computed(() => route.path.startsWith("/todos"));
const isProfileActive = computed(() => route.path.startsWith("/profile"));
const isRegisterActive = computed(() => route.path === "/register");
const isLoginActive = computed(() => route.path === "/login");

const linkClass = (isActive: boolean) =>
  [
    "rounded-md px-2 py-1 transition",
    isActive
      ? "bg-slate-900 text-white"
      : "text-slate-600 hover:bg-slate-100 hover:text-slate-900",
  ].join(" ");
</script>

<template>
  <header
    class="sticky top-0 z-10 border-b border-slate-200 bg-white/95 backdrop-blur"
  >
    <div
      class="mx-auto flex w-full max-w-6xl items-center justify-between px-4 py-3 md:px-6"
    >
      <RouterLink to="/" class="text-lg font-bold text-slate-900"
        >Todo App</RouterLink
      >

      <nav class="flex items-center gap-4 text-sm font-medium">
        <RouterLink to="/" custom v-slot="{ href, navigate }">
          <a :href="href" :class="linkClass(isHomeActive)" @click="navigate"
            >Home</a
          >
        </RouterLink>

        <template v-if="authStore.isAuthenticated">
          <RouterLink to="/todos" custom v-slot="{ href, navigate }">
            <a :href="href" :class="linkClass(isTodosActive)" @click="navigate"
              >Todos</a
            >
          </RouterLink>
          <RouterLink to="/profile" custom v-slot="{ href, navigate }">
            <a
              :href="href"
              :class="linkClass(isProfileActive)"
              @click="navigate"
              >Profile</a
            >
          </RouterLink>
          <button
            type="button"
            class="rounded-md bg-slate-900 px-3 py-1.5 text-white transition hover:bg-slate-800"
            @click="authStore.logout"
          >
            Logout
          </button>
        </template>

        <template v-else>
          <RouterLink to="/register" custom v-slot="{ href, navigate }">
            <a
              :href="href"
              :class="linkClass(isRegisterActive)"
              @click="navigate"
              >Register</a
            >
          </RouterLink>
          <RouterLink to="/login" custom v-slot="{ href, navigate }">
            <a
              :href="href"
              :class="[
                'rounded-md px-3 py-1.5 transition',
                isLoginActive
                  ? 'bg-blue-700 text-white'
                  : 'bg-blue-600 text-white hover:bg-blue-700',
              ]"
              @click="navigate"
              >Login</a
            >
          </RouterLink>
        </template>
      </nav>
    </div>
  </header>
</template>
