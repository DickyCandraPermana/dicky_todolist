<script setup lang="ts">
import { onMounted } from "vue";
import { RouterLink } from "vue-router";
import { useTodoStore } from "@/stores/todoStore";
import { Button } from "@/components/button";

const todoStore = useTodoStore();

onMounted(async () => {
  await todoStore.fetchTodos();
});

const handleComplete = async (id: string) => {
  await todoStore.completeTodo(id);
};

const handleDelete = async (id: string) => {
  await todoStore.deleteTodo(id);
};
</script>

<template>
  <div class="mx-auto w-full max-w-4xl px-4 py-10">
    <div class="mb-6 flex items-center justify-between">
      <h1 class="text-2xl font-bold text-slate-900">Todo Saya</h1>
      <RouterLink to="/todos/create">
        <Button size="sm">Tambah Todo</Button>
      </RouterLink>
    </div>

    <p v-if="todoStore.error" class="mb-4 text-sm text-red-600">
      {{ todoStore.error }}
    </p>

    <div
      v-if="todoStore.isLoading"
      class="rounded-lg border border-slate-200 bg-white p-4 text-sm text-slate-600"
    >
      Memuat todo...
    </div>

    <div
      v-else-if="todoStore.todos.length === 0"
      class="rounded-lg border border-dashed border-slate-300 bg-white p-8 text-center text-sm text-slate-600"
    >
      Belum ada todo.
    </div>

    <div v-else class="space-y-3">
      <article
        v-for="todo in todoStore.todos"
        :key="todo.id"
        class="rounded-lg border border-slate-200 bg-white p-4 shadow-sm"
      >
        <div class="flex flex-wrap items-start justify-between gap-3">
          <div class="flex-1">
            <h2
              class="text-base font-semibold"
              :class="
                todo.isCompleted
                  ? 'text-slate-500 line-through'
                  : 'text-slate-900'
              "
            >
              {{ todo.title }}
            </h2>
            <p class="mt-1 text-sm text-slate-600">
              {{ todo.description || "-" }}
            </p>
          </div>

          <div class="flex flex-wrap gap-2">
            <RouterLink :to="`/todos/${todo.id}/edit`">
              <Button variant="secondary" size="sm">Edit</Button>
            </RouterLink>
            <Button
              v-if="!todo.isCompleted"
              variant="primary"
              size="sm"
              @click="handleComplete(todo.id)"
            >
              Selesai
            </Button>
            <Button variant="danger" size="sm" @click="handleDelete(todo.id)">
              Hapus
            </Button>
          </div>
        </div>
      </article>
    </div>
  </div>
</template>
