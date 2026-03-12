<script lang="ts" setup>
import Footer from "@/components/footer";
import Navbar from "@/components/navbar";
import { RouterView } from "vue-router";
</script>

<template>
  <div class="flex min-h-screen flex-col bg-slate-50 text-slate-900">
    <Navbar />
    <main class="mx-auto w-full max-w-6xl flex-1 px-4 py-6 md:px-6">
      <RouterView />
    </main>
    <Footer />
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease-in-out;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
