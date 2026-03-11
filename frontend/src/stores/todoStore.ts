import { defineStore } from "pinia";
import { ref } from "vue";
import { TodoService } from "@/services/TodoService";
import type { CreateTodoRequest, Todo, UpdateTodoRequest } from "@/types/todo";

export const useTodoStore = defineStore("todo", () => {
  const todos = ref<Todo[]>([]);
  const isLoading = ref(false);
  const error = ref<string>("");

  async function fetchTodos() {
    isLoading.value = true;
    error.value = "";

    try {
      const data = await TodoService.getAll();
      todos.value = data;
    } catch (err: any) {
      error.value = err.response?.data?.message || "Gagal mengambil data todo.";
      throw err;
    } finally {
      isLoading.value = false;
    }
  }

  async function getTodoById(id: string) {
    return TodoService.getById(id);
  }

  async function createTodo(payload: CreateTodoRequest) {
    const created = await TodoService.create(payload);
    todos.value = [created, ...todos.value];
    return created;
  }

  async function updateTodo(id: string, payload: UpdateTodoRequest) {
    const updated = await TodoService.update(id, payload);
    todos.value = todos.value.map((todo) => (todo.id === id ? updated : todo));
    return updated;
  }

  async function completeTodo(id: string) {
    const completed = await TodoService.complete(id);
    todos.value = todos.value.map((todo) =>
      todo.id === id ? completed : todo,
    );
    return completed;
  }

  async function deleteTodo(id: string) {
    await TodoService.remove(id);
    todos.value = todos.value.filter((todo) => todo.id !== id);
  }

  return {
    todos,
    isLoading,
    error,
    fetchTodos,
    getTodoById,
    createTodo,
    updateTodo,
    completeTodo,
    deleteTodo,
  };
});
