import { defineStore } from "pinia";
import { ref } from "vue";
import { UserService } from "@/services/UserService";
import type { UpdateUserRequest, User } from "@/types/user";

export const useUserStore = defineStore("user", () => {
  const isLoading = ref(false);
  const error = ref("");

  async function getById(id: string): Promise<User> {
    isLoading.value = true;
    error.value = "";

    try {
      return await UserService.getById(id);
    } catch (err: any) {
      error.value = err.response?.data?.message || "Gagal mengambil data user.";
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function updateUser(
    id: string,
    payload: UpdateUserRequest,
  ): Promise<User> {
    isLoading.value = true;
    error.value = "";

    try {
      return await UserService.update(id, payload);
    } catch (err: any) {
      error.value = err.response?.data?.message || "Gagal memperbarui profil.";
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function deleteUser(id: string): Promise<void> {
    isLoading.value = true;
    error.value = "";

    try {
      await UserService.remove(id);
    } catch (err: any) {
      error.value = err.response?.data?.message || "Gagal menghapus akun.";
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    isLoading,
    error,
    getById,
    updateUser,
    deleteUser,
  };
});
