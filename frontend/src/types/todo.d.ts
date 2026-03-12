export interface Todo {
  id: string;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
  updatedAt: string;
  dueDate?: string | null;
  deletedAt?: string | null; // Mewakili Soft Delete
}

export interface CreateTodoRequest {
  title: string;
  description?: string;
  dueDate?: string | null;
}

export interface UpdateTodoRequest {
  title?: string;
  description?: string;
  isCompleted?: boolean;
  dueDate?: string | null;
}
