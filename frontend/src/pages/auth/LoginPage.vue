<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/authStore";

const router = useRouter();
const authStore = useAuthStore();

// State form
const email = ref("");
const password = ref("");
const isLoading = ref(false);
const error = ref("");

const handleLogin = async () => {
  error.value = "";
  isLoading.value = true;

  try {
    // Memanggil action dari store
    await authStore.login({
      email: email.value,
      password: password.value,
    });

    // Sukses: Redirect ke dashboard todo
    router.push("/profile");
  } catch (err: any) {
    // Menangkap error dari Global Exception Handling
    error.value =
      err.response?.data?.message || "Login gagal, periksa kredensial Anda.";
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="login-container">
    <form @submit.prevent="handleLogin" class="login-card">
      <h1>Login</h1>

      <p v-if="error" class="error-message">{{ error }}</p>

      <div class="field">
        <label>Email</label>
        <input v-model="email" type="email" required />
      </div>

      <div class="field">
        <label>Password</label>
        <input v-model="password" type="password" required />
      </div>

      <button type="submit" :disabled="isLoading">
        {{ isLoading ? "Memproses..." : "Masuk" }}
      </button>
    </form>
  </div>
</template>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.login-card {
  padding: 2rem;
  border-radius: 8px;
  background: #f4f4f4;
  width: 100%;
  max-width: 400px;
}

.field {
  margin-bottom: 1rem;
}
.error-message {
  color: red;
  font-size: 0.9rem;
}
button {
  width: 100%;
  padding: 0.8rem;
}
</style>
