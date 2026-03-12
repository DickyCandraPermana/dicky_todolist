<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useTodoStore } from "@/stores/todoStore";
import { Button } from "@/components/button";

const route = useRoute();
const router = useRouter();
const todoStore = useTodoStore();

const todoId = computed(() => String(route.params.id || ""));
const isEditMode = computed(() => !!route.params.id);

const title = ref("");
const description = ref("");
const dueDate = ref("");
const isCompleted = ref(false);
const isLoading = ref(false);
const error = ref("");

onMounted(async () => {
  if (!isEditMode.value) return;

  isLoading.value = true;
  error.value = "";

  try {
    const todo = await todoStore.getTodoById(todoId.value);
    title.value = todo.title || "";
    description.value = todo.description || "";
    isCompleted.value = todo.isCompleted;
    // Format YYYY-MM-DD for input type="date"
    if (todo.dueDate) {
      dueDate.value = new Date(todo.dueDate).toISOString().split("T")[0] || "";
    }
  } catch (err: any) {
    error.value = err.response?.data?.message || "Gagal mengambil detail todo.";
  } finally {
    isLoading.value = false;
  }
});

const handleSubmit = async () => {
  error.value = "";
  isLoading.value = true;

  try {
    if (isEditMode.value) {
      await todoStore.updateTodo(todoId.value, {
        title: title.value,
        description: description.value,
        isCompleted: isCompleted.value,
        dueDate: dueDate.value || null,
      });
    } else {
      await todoStore.createTodo({
        title: title.value,
        description: description.value,
        dueDate: dueDate.value || null,
      });
    }

    router.push("/todos");
  } catch (err: any) {
    error.value = err.response?.data?.message || "Gagal menyimpan todo.";
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="mx-auto w-full max-w-3xl px-4 py-10">
    <form
      @submit.prevent="handleSubmit"
      class="rounded-xl border border-slate-200 bg-white p-6 shadow-sm"
    >
      <h1 class="mb-6 text-2xl font-bold text-slate-900">
        {{ isEditMode ? "Edit Todo" : "Tambah Todo" }}
      </h1>

      <p v-if="error" class="mb-4 text-sm text-red-600">{{ error }}</p>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700"
          >Judul</label
        >
        <input
          v-model="title"
          type="text"
          required
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700"
          >Deskripsi</label
        >
        <textarea
          v-model="description"
          rows="4"
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <div class="mb-4">
        <label class="mb-1 block text-sm font-medium text-slate-700"
          >Tenggat Waktu</label
        >
        <input
          v-model="dueDate"
          type="date"
          class="w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 outline-none ring-blue-500 transition focus:ring-2"
        />
      </div>

      <div v-if="isEditMode" class="mb-6 flex items-center gap-2">
        <input id="isCompleted" v-model="isCompleted" type="checkbox" />
        <label for="isCompleted" class="text-sm text-slate-700">Selesai</label>
      </div>

      <div class="flex flex-wrap gap-3">
        <Button type="submit" :disabled="isLoading" :loading="isLoading">
          {{ isLoading ? "Menyimpan..." : "Simpan" }}
        </Button>
        <Button
          type="button"
          variant="secondary"
          @click="router.push('/todos')"
        >
          Batal
        </Button>
      </div>
    </form>
  </div>
</template>
