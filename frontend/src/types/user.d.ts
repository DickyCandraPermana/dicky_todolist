export interface User {
  id: string;
  username: string;
  email: string;
  password?: string;
  createdAt: string;
  updatedAt: string;
  deletedAt?: string | null;
}

export interface UpdateUserRequest {
  username?: string;
  email?: string;
  password?: string;
}
