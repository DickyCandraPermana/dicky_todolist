<template>
  <GuestHero v-if="!authStore.isAuthenticated" />

  <section v-else class="py-8">
    <DashboardHeader :username="authStore.user?.username ?? ''">
      <Button size="lg">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round" class="mr-2"><path d="M5 12h14"/><path d="M12 5v14"/></svg>
        Todo Baru
      </Button>
    </DashboardHeader>

    <StatsCards :stats="todoStore.stats" />

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <TaskStatusChart :stats="todoStore.stats" :isLoading="isLoading" />
      <WeeklyProgressChart :chartData="chartData" :isLoading="isLoading" />
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useAuthStore } from '@/stores/authStore';
import { useTodoStore } from '@/stores/todoStore';
import { Button } from '@/components/button';
import GuestHero from '@/components/dashboard/GuestHero.vue';
import DashboardHeader from '@/components/dashboard/DashboardHeader.vue';
import StatsCards from '@/components/dashboard/StatsCards.vue';
import TaskStatusChart from '@/components/dashboard/TaskStatusChart.vue';
import WeeklyProgressChart from '@/components/dashboard/WeeklyProgressChart.vue';

const authStore = useAuthStore();
const todoStore = useTodoStore();

const isLoading = ref(true);
const chartData = ref<{ date: string; count: number }[]>([]);

onMounted(async () => {
  if (authStore.isAuthenticated) {
    try {
      isLoading.value = true;
      await Promise.all([
        todoStore.fetchProductivity().then(data => chartData.value = data),
        todoStore.fetchStats()
      ]);
    } catch (error) {
      console.error('Gagal memuat data dashboard:', error);
    } finally {
      isLoading.value = false;
    }
  } else {
    isLoading.value = false;
  }
});
</script>
