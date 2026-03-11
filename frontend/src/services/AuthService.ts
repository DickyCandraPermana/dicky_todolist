import api from "@/api/axios";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from "@/types/auth";
import type { User } from "@/types/user";

export const AuthService = {
  async login(payload: LoginRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>("/api/Auth/login", payload);
    return response.data;
  },

  async register(payload: RegisterRequest): Promise<void> {
    await api.post("/api/Auth/register", payload);
  },

  async getProfile(): Promise<User> {
    const response = await api.post<User>("/api/Auth/profile");
    return response.data;
  },
};
