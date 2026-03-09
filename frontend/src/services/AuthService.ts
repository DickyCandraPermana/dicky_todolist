import api from "@/api/axios";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from "@/types/auth";

export const AuthService = {
  async login(payload: LoginRequest): Promise<LoginResponse> {
    const response = await api.post<LoginResponse>("/api/Auth/login", payload);
    return response.data;
  },

  async register(payload: RegisterRequest): Promise<void> {
    await api.post("/api/Auth/register", payload);
  },

  async getProfile(): Promise<any> {
    const response = await api.get("/api/Auth/profile");
    return response.data;
  },
};
