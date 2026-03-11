import api from "@/api/axios";
import type { CreateTodoRequest, Todo, UpdateTodoRequest } from "@/types/todo";

export const TodoService = {
  async getAll(): Promise<Todo[]> {
    const response = await api.get<Todo[]>("/api/Todos");
    return response.data;
  },

  async getById(id: string): Promise<Todo> {
    const response = await api.get<Todo>(`/api/Todos/${id}`);
    return response.data;
  },

  async create(payload: CreateTodoRequest): Promise<Todo> {
    const response = await api.post<Todo>("/api/Todos", payload);
    return response.data;
  },

  async update(id: string, payload: UpdateTodoRequest): Promise<Todo> {
    const response = await api.put<Todo>(`/api/Todos/${id}`, payload);
    return response.data;
  },

  async complete(id: string): Promise<Todo> {
    const response = await api.patch<Todo>(`/api/Todos/${id}/complete`);
    return response.data;
  },

  async remove(id: string): Promise<void> {
    await api.delete(`/api/Todos/${id}`);
  },
};
