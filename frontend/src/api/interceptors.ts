import api from "./axios";
import { useAuthStore } from "@/stores/authStore";

export const setupInterceptors = () => {
  api.interceptors.request.use((config) => {
    const authStore = useAuthStore(); // Panggil di dalam fungsi agar tidak melanggar aturan Pinia
    if (authStore.token) {
      config.headers.Authorization = `Bearer ${authStore.token}`;
    }
    return config;
  });

  api.interceptors.response.use(
    (response) => response,
    (error) => {
      const authStore = useAuthStore();
      if (error.response?.status === 401) {
        authStore.logout(); // Menangani Authorization secara global
      }
      return Promise.reject(error);
    },
  );
};
