<script setup lang="ts">
import { ref } from "vue";
import { RouterLink, useRouter } from "vue-router";
import { AuthService } from "@/services/AuthService";
import { Button } from "@/components/button";

const router = useRouter();

const username = ref("");
const email = ref("");
const password = ref("");
const isLoading = ref(false);
const error = ref("");

const handleRegister = async () => {
  error.value = "";
  isLoading.value = true;

  try {
    await AuthService.register({
      username: username.value,
      email: email.value,
      password: password.value,
    });

    router.push("/login");
  } catch (err: any) {
    error.value = err.response?.data?.message || "Registrasi gagal.";
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex min-h-screen items-center justify-center px-4">
    <form
      @submit.prevent="handleRegister"
      class="w-full max-w-md rounded-lg bg-slate-100 p-8 shadow"
    >
      <h1 class="mb-6 text-2xl font-bold text-slate-900">Register</h1>

      <p v-if="error" class="mb-4 text-sm text-red-600">{{ error }}</p>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700"
          >Username</label
        >
        <input
          v-model="username"
          type="text"
          minlength="3"
          required
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700"
          >Email</label
        >
        <input
          v-model="email"
          type="email"
          required
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <div class="mb-6">
        <label class="mb-1 block text-sm font-medium text-slate-700"
          >Password</label
        >
        <input
          v-model="password"
          type="password"
          minlength="6"
          required
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <Button type="submit" :disabled="isLoading" :loading="isLoading" block>
        {{ isLoading ? "Memproses..." : "Daftar" }}
      </Button>

      <p class="mt-4 text-center text-sm text-slate-600">
        Sudah punya akun?
        <RouterLink
          to="/login"
          class="font-medium text-blue-600 hover:underline"
          >Masuk</RouterLink
        >
      </p>
    </form>
  </div>
</template>
