<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  chartData: { date: string; count: number }[]
  isLoading: boolean
}>()

const series = computed(() => [{
  name: 'Todo Selesai',
  data: props.chartData.map(item => item.count)
}])

const options = computed(() => ({
  chart: {
    type: 'area',
    toolbar: { show: false },
    zoom: { enabled: false },
    fontFamily: 'inherit'
  },
  dataLabels: { enabled: false },
  stroke: { curve: 'smooth', width: 3, colors: ['#2563eb'] },
  fill: {
    type: 'gradient',
    gradient: {
      shadeIntensity: 1,
      opacityFrom: 0.45,
      opacityTo: 0.05,
      stops: [20, 100],
      colorStops: [
        { offset: 0, color: '#2563eb', opacity: 0.4 },
        { offset: 100, color: '#2563eb', opacity: 0 }
      ]
    }
  },
  xaxis: {
    categories: props.chartData.map(item => {
      const date = new Date(item.date);
      return date.toLocaleDateString('id-ID', { weekday: 'short' });
    }),
    axisBorder: { show: false },
    axisTicks: { show: false },
    labels: { style: { colors: '#64748b', fontWeight: 500 } }
  },
  yaxis: {
    min: 0,
    forceNiceScale: true,
    labels: {
      style: { colors: '#64748b', fontWeight: 500 },
      formatter: (val: number) => Math.floor(val)
    }
  },
  grid: {
    borderColor: '#f1f5f9',
    strokeDashArray: 4,
    padding: { left: 20, right: 20 }
  },
  colors: ['#2563eb'],
  markers: {
    size: 4,
    colors: ['#2563eb'],
    strokeColors: '#fff',
    strokeWidth: 2,
    hover: { size: 6 }
  },
  tooltip: {
    theme: 'light',
    x: {
      formatter: (_val: string, { dataPointIndex }: any) => {
        const item = props.chartData[dataPointIndex];
        return item ? new Date(item.date).toLocaleDateString('id-ID', { dateStyle: 'long' }) : '';
      }
    }
  }
}))
</script>

<template>
  <article class="lg:col-span-2 rounded-lg border border-slate-200 bg-white p-6 md:p-8 shadow-sm">
    <div class="mb-6">
      <h2 class="text-lg font-bold text-slate-800">Progres Mingguan</h2>
    </div>

    <div v-if="isLoading" class="flex h-[350px] items-center justify-center">
      <div class="h-10 w-10 animate-spin rounded-full border-4 border-slate-100 border-t-blue-600"></div>
    </div>

    <div v-else-if="chartData.length > 0" class="min-h-[350px]">
      <apexchart type="area" height="350" :options="options" :series="series"></apexchart>
    </div>

    <div v-else class="flex h-[350px] flex-col items-center justify-center text-slate-400">
      <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round" class="mb-4 opacity-20">
        <rect width="18" height="18" x="3" y="4" rx="2" ry="2"/>
        <line x1="16" x2="16" y1="2" y2="6"/>
        <line x1="8" x2="8" y1="2" y2="6"/>
        <line x1="3" x2="21" y1="10" y2="10"/>
        <path d="M8 14h.01"/><path d="M12 14h.01"/><path d="M16 14h.01"/>
        <path d="M8 18h.01"/><path d="M12 18h.01"/><path d="M16 18h.01"/>
      </svg>
      <p class="font-medium">Belum ada aktivitas tercatat.</p>
    </div>
  </article>
</template>
