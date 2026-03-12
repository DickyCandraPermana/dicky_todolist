<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { RouterLink, useRoute } from "vue-router";
import { useAuthStore } from "@/stores/authStore";

const authStore = useAuthStore();
const route = useRoute();
const mobileMenuOpen = ref(false);

const isHomeActive = computed(() => route.path === "/");
const isTodosActive = computed(() => route.path.startsWith("/todos"));
const isProfileActive = computed(() => route.path.startsWith("/profile"));
const isRegisterActive = computed(() => route.path === "/register");
const isLoginActive = computed(() => route.path === "/login");

watch(
  () => route.path,
  () => {
    mobileMenuOpen.value = false;
  },
);

const toggleMobileMenu = () => {
  mobileMenuOpen.value = !mobileMenuOpen.value;
};

const closeMobileMenu = () => {
  mobileMenuOpen.value = false;
};

const handleLogout = () => {
  closeMobileMenu();
  authStore.logout();
};

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

      <button
        type="button"
        class="inline-flex items-center rounded-md border border-slate-200 p-2 text-slate-700 transition hover:bg-slate-100 md:hidden"
        :aria-expanded="mobileMenuOpen"
        aria-label="Toggle menu"
        @click="toggleMobileMenu"
      >
        <span v-if="!mobileMenuOpen">☰</span>
        <span v-else>✕</span>
      </button>

      <nav class="hidden items-center gap-4 text-sm font-medium md:flex">
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
            @click="handleLogout"
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

    <nav
      v-if="mobileMenuOpen"
      class="mx-auto w-full max-w-6xl space-y-2 border-t border-slate-200 px-4 py-3 text-sm font-medium md:hidden"
    >
      <RouterLink to="/" custom v-slot="{ href, navigate }">
        <a
          :href="href"
          :class="['block', linkClass(isHomeActive)]"
          @click="navigate"
          >Home</a
        >
      </RouterLink>

      <template v-if="authStore.isAuthenticated">
        <RouterLink to="/todos" custom v-slot="{ href, navigate }">
          <a
            :href="href"
            :class="['block', linkClass(isTodosActive)]"
            @click="navigate"
            >Todos</a
          >
        </RouterLink>
        <RouterLink to="/profile" custom v-slot="{ href, navigate }">
          <a
            :href="href"
            :class="['block', linkClass(isProfileActive)]"
            @click="navigate"
            >Profile</a
          >
        </RouterLink>
        <button
          type="button"
          class="w-full rounded-md bg-slate-900 px-3 py-2 text-left text-white transition hover:bg-slate-800"
          @click="handleLogout"
        >
          Logout
        </button>
      </template>

      <template v-else>
        <RouterLink to="/register" custom v-slot="{ href, navigate }">
          <a
            :href="href"
            :class="['block', linkClass(isRegisterActive)]"
            @click="navigate"
            >Register</a
          >
        </RouterLink>
        <RouterLink to="/login" custom v-slot="{ href, navigate }">
          <a
            :href="href"
            :class="[
              'block rounded-md px-3 py-2 transition',
              isLoginActive
                ? 'bg-blue-700 text-white'
                : 'bg-blue-600 text-white hover:bg-blue-700',
            ]"
            @click="navigate"
            >Login</a
          >
        </RouterLink>
      </template>

      <button
        type="button"
        class="w-full rounded-md border border-slate-200 px-3 py-2 text-left text-slate-600 transition hover:bg-slate-100"
        @click="closeMobileMenu"
      >
        Tutup Menu
      </button>
    </nav>
  </header>
</template>
