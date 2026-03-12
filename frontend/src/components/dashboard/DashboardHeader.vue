<script setup lang="ts">
defineProps<{
  username: string
}>()
</script>

<template>
  <div class="mb-10 flex flex-col md:flex-row md:items-center justify-between gap-4">
    <div>
      <h1 class="text-2xl font-bold text-slate-900 leading-tight">
        Selamat Datang{{ username ? ', ' + username : '' }}
      </h1>
      <p class="mt-1 text-sm text-slate-600">Berikut ringkasan produktivitasmu 7 hari terakhir.</p>
    </div>
    <router-link to="/todos/create">
      <slot />
    </router-link>
  </div>
</template>
