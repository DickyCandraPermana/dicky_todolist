<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { RouterLink } from "vue-router";
import { AuthService } from "@/services/AuthService";
import { Button } from "@/components/button";

const router = useRouter();

// State form
const username = ref("");
const email = ref("");
const password = ref("");
const isLoading = ref(false);
const error = ref("");
const success = ref("");

const handleRegister = async () => {
  error.value = "";
  success.value = "";
  isLoading.value = true;

  try {
    // Memanggil AuthService untuk registrasi
    await AuthService.register({
      username: username.value,
      email: email.value,
      password: password.value,
    });

    success.value = "Registrasi berhasil! Mengalihkan ke halaman login...";
    
    // Redirect ke login setelah delay singkat
    setTimeout(() => {
      router.push("/login");
    }, 2000);
  } catch (err: any) {
    // Menangkap error dari backend
    error.value = err.response?.data?.message || "Registrasi gagal, silakan coba lagi.";
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
      <h1 class="mb-6 text-2xl font-bold text-slate-900">Daftar Akun</h1>

      <p v-if="error" class="mb-4 text-sm text-red-600">{{ error }}</p>
      <p v-if="success" class="mb-4 text-sm text-green-600">{{ success }}</p>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700">Username</label>
        <input
          v-model="username"
          type="text"
          required
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700">Email</label>
        <input
          v-model="email"
          type="email"
          required
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <div class="mb-6">
        <label class="mb-1 block text-sm font-medium text-slate-700">Password</label>
        <input
          v-model="password"
          type="password"
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
          >Masuk di sini</RouterLink
        >
      </p>
    </form>
  </div>
</template>
