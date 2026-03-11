<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { RouterLink } from "vue-router";
import { useAuthStore } from "@/stores/authStore";
import { useUserStore } from "@/stores/userStore";
import { Button } from "@/components/button";

const authStore = useAuthStore();
const userStore = useUserStore();

const username = ref("");
const email = ref("");
const password = ref("");
const updateError = ref("");

const syncForm = () => {
  username.value = authStore.user?.username || "";
  email.value = authStore.user?.email || "";
  password.value = "";
};

onMounted(async () => {
  if (!authStore.user) {
    await authStore.fetchProfile();
  }

  syncForm();
});

const createdAtLabel = computed(() => {
  if (!authStore.user?.createdAt) return "-";

  return new Date(authStore.user.createdAt).toLocaleString("id-ID", {
    dateStyle: "medium",
    timeStyle: "short",
  });
});

const handleUpdate = async () => {
  updateError.value = "";

  try {
    if (!authStore.user) return;

    const payload = {
      username: username.value,
      email: email.value,
      password: password.value || undefined,
    };

    const updatedUser = await userStore.updateUser(authStore.user.id, payload);
    authStore.user = updatedUser;
    syncForm();
  } catch {
    updateError.value = userStore.error || "Gagal memperbarui profil.";
  }
};

const handleDelete = async () => {
  if (!authStore.user) return;

  const confirmed = window.confirm("Yakin ingin menghapus akun?");
  if (!confirmed) return;

  try {
    await userStore.deleteUser(authStore.user.id);
    authStore.logout();
  } catch {
    updateError.value = userStore.error || "Gagal menghapus akun.";
  }
};
</script>

<template>
  <div class="mx-auto w-full max-w-3xl px-4 py-10">
    <div class="rounded-xl border border-slate-200 bg-white p-6 shadow-sm">
      <h1 class="mb-6 text-2xl font-bold text-slate-900">Profil</h1>

      <div class="space-y-4 text-sm text-slate-700">
        <div>
          <p class="text-slate-500">Username</p>
          <p class="font-medium text-slate-900">
            {{ authStore.user?.username || "-" }}
          </p>
        </div>

        <div>
          <p class="text-slate-500">Email</p>
          <p class="font-medium text-slate-900">
            {{ authStore.user?.email || "-" }}
          </p>
        </div>

        <div>
          <p class="text-slate-500">Bergabung sejak</p>
          <p class="font-medium text-slate-900">{{ createdAtLabel }}</p>
        </div>
      </div>

      <form class="mt-8 space-y-4" @submit.prevent="handleUpdate">
        <p v-if="updateError" class="text-sm text-red-600">{{ updateError }}</p>

        <div>
          <label class="mb-1 block text-sm font-medium text-slate-700"
            >Username baru</label
          >
          <input
            v-model="username"
            type="text"
            minlength="3"
            required
            class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
          />
        </div>

        <div>
          <label class="mb-1 block text-sm font-medium text-slate-700"
            >Email baru</label
          >
          <input
            v-model="email"
            type="email"
            required
            class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
          />
        </div>

        <div>
          <label class="mb-1 block text-sm font-medium text-slate-700"
            >Password baru (opsional)</label
          >
          <input
            v-model="password"
            type="password"
            minlength="6"
            class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
          />
        </div>

        <Button
          type="submit"
          :disabled="userStore.isLoading"
          :loading="userStore.isLoading"
        >
          Simpan Perubahan
        </Button>
      </form>

      <div class="mt-8 flex flex-wrap gap-3 border-t border-slate-200 pt-6">
        <RouterLink to="/todos">
          <Button>Kelola Todo</Button>
        </RouterLink>
        <Button variant="secondary" @click="authStore.logout">Logout</Button>
        <Button
          variant="danger"
          :disabled="userStore.isLoading"
          @click="handleDelete"
          >Hapus Akun</Button
        >
      </div>
    </div>
  </div>
</template>
