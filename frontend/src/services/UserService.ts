import api from "@/api/axios";
import type { UpdateUserRequest, User } from "@/types/user";

export const UserService = {
  async getAll(): Promise<User[]> {
    const response = await api.get<User[]>("/api/Users");
    return response.data;
  },

  async getById(id: string): Promise<User> {
    const response = await api.get<User>(`/api/Users/${id}`);
    return response.data;
  },

  async update(id: string, payload: UpdateUserRequest): Promise<User> {
    const response = await api.patch<User>(`/api/Users/${id}`, payload);
    return response.data;
  },

  async remove(id: string): Promise<void> {
    await api.delete(`/api/Users/${id}`);
  },
};
