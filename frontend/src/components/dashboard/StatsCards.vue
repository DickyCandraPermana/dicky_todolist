<script setup lang="ts">
defineProps<{
  stats: {
    totalTasks: number
    pendingTasks: number
    completedTasks: number
    completedToday: number
  } | null
}>()
</script>

<template>
  <div class="mb-10 grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
    <article class="rounded-lg border border-slate-200 bg-white p-6 shadow-sm">
      <p class="text-xs font-medium text-slate-500 uppercase tracking-wider">Total Tugas</p>
      <h3 class="mt-1 text-xl font-bold text-slate-900">{{ stats?.totalTasks || 0 }}</h3>
    </article>

    <article class="rounded-lg border border-slate-200 bg-white p-6 shadow-sm">
      <p class="text-xs font-medium text-slate-500 uppercase tracking-wider">Belum Selesai</p>
      <h3 class="mt-1 text-xl font-bold text-slate-900">{{ stats?.pendingTasks || 0 }}</h3>
    </article>

    <article class="rounded-lg border border-slate-200 bg-white p-6 shadow-sm">
      <p class="text-xs font-medium text-slate-500 uppercase tracking-wider">Selesai</p>
      <h3 class="mt-1 text-xl font-bold text-slate-900">{{ stats?.completedTasks || 0 }}</h3>
    </article>

    <article class="rounded-lg border border-slate-200 bg-white p-6 shadow-sm">
      <p class="text-xs font-medium text-slate-500 uppercase tracking-wider">Selesai Hari Ini</p>
      <h3 class="mt-1 text-xl font-bold text-slate-900">{{ stats?.completedToday || 0 }}</h3>
    </article>
  </div>
</template>
