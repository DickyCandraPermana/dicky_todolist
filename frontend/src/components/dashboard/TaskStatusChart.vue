<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  stats: { completedTasks: number; pendingTasks: number } | null
  isLoading: boolean
}>()

const series = computed(() => [
  props.stats?.completedTasks || 0,
  props.stats?.pendingTasks || 0
])

const options = computed(() => ({
  chart: { type: 'donut', fontFamily: 'inherit' },
  labels: ['Selesai', 'Belum Selesai'],
  colors: ['#10b981', '#f59e0b'],
  plotOptions: {
    pie: {
      donut: {
        size: '75%',
        labels: {
          show: true,
          total: {
            show: true,
            label: 'Total',
            fontSize: '16px',
            fontWeight: 700,
            color: '#64748b'
          }
        }
      }
    }
  },
  dataLabels: { enabled: false },
  legend: { position: 'bottom', fontWeight: 600, markers: { radius: 12 } },
  stroke: { show: false }
}))
</script>

<template>
  <article class="lg:col-span-1 rounded-lg border border-slate-200 bg-white p-6 md:p-8 shadow-sm">
    <h2 class="text-lg font-bold text-slate-800 mb-6">Status Tugas</h2>

    <div v-if="isLoading" class="flex h-[350px] items-center justify-center">
      <div class="h-10 w-10 animate-spin rounded-full border-4 border-slate-100 border-t-blue-600"></div>
    </div>

    <div v-else-if="stats" class="flex h-[350px] items-center justify-center">
      <apexchart type="donut" width="100%" :options="options" :series="series"></apexchart>
    </div>
  </article>
</template>
