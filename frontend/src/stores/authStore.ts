import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { AuthService } from "@/services/AuthService";
import type { LoginRequest } from "@/types/auth";
import type { User } from "@/types/user";

export const useAuthStore = defineStore("auth", () => {
  // State
  const user = ref<User | null>(null);
  const token = ref<string | null>(localStorage.getItem("access_token"));

  // Getters
  const isAuthenticated = computed(() => !!token.value);

  // Actions
  async function login(payload: LoginRequest) {
    try {
      const data = await AuthService.login(payload);

      // Update State
      token.value = data.token;
      user.value = data.user;

      // Simpan ke localStorage agar tidak hilang saat refresh
      localStorage.setItem("access_token", data.token);

      return data;
    } catch (error) {
      logout();
      throw error;
    }
  }

  function logout() {
    token.value = null;
    user.value = null;
    localStorage.removeItem("access_token");
    window.location.href = "/login";
  }

  async function fetchProfile() {
    if (!token.value) return;
    try {
      const userData = await AuthService.getProfile();
      user.value = userData;
    } catch (error) {
      logout();
    }
  }

  return {
    user,
    token,
    isAuthenticated,
    login,
    logout,
    fetchProfile,
  };
});
