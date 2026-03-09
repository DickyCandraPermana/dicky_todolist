<script setup lang="ts">
import { onMounted } from "vue";
import { useAuthStore } from "@/stores/authStore";

const authStore = useAuthStore();

onMounted(async () => {
  if (authStore.token) {
    await authStore.fetchProfile();
  }
});
</script>

<template>
  <div id="app">
    <RouterView v-slot="{ Component }">
      <transition name="fade" mode="out-in">
        <component :is="Component" />
      </transition>
    </RouterView>
  </div>
</template>
