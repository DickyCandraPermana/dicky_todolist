export interface Todo {
  id: string;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
  updatedAt: string;
  deletedAt?: string | null; // Mewakili Soft Delete
}

export interface CreateTodoRequest {
  title: string;
  description?: string;
}

export interface UpdateTodoRequest {
  title?: string;
  description?: string;
  isCompleted?: boolean;
}
