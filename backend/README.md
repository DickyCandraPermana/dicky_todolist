# Dicky TodoList API 

API pengelola daftar tugas (Todo List) yang dibangun dengan prinsip **Clean Code** dan **Best Practices** untuk memenuhi _Technical Test Backend Intern_.

## ✨ Fitur Utama (Best Practices)

Aplikasi ini tidak hanya sekadar CRUD, melainkan sudah dilengkapi dengan:

- **Authentication & Authorization**: Menggunakan **JWT (JSON Web Token)**.
- **Data Ownership**: Setiap user hanya dapat mengelola data miliknya sendiri.
- **Global Exception Handling**: Middleware terpusat untuk menangani error secara konsisten (Format JSON).
- **Service Pattern**: Memisahkan logika bisnis dari Controller.
- **FluentValidation**: Validasi input yang ketat dan terpisah dari Model.
- **Soft Delete**: Data tidak benar-benar dihapus dari database, melainkan ditandai dengan `DeletedAt`.
- **Global Query Filter**: Secara otomatis menyaring data yang sudah dihapus di seluruh aplikasi.

---

## 🛠️ Teknologi & Library

- **Framework:** ASP.NET Core 8 (Web API)
- **ORM:** Entity Framework Core
- **Database:** SQLite
- **Security:** JWT Bearer & BCrypt Password Hashing
- **Validation:** FluentValidation
- **API Documentation:** Swagger UI (Swashbuckle)

---

## 📂 Struktur Folder

```text
dicky_todolist/
├── Controllers/    # Entry point & Handling request
├── Services/       # Logika bisnis (Auth, dll)
├── Data/           # Konfigurasi DbContext & Global Filters
├── Models/         # Entity Database
├── DTOs/           # Data Transfer Objects untuk Request/Response
├── Middlewares/    # Global Exception Handling
├── Validators/     # Aturan validasi (FluentValidation)
└── Exceptions/     # Custom Exception classes

```

---

## 🚀 Cara Menjalankan Project

### 1. Persiapan

```bash
git clone https://github.com/DickyCandraPermana/dicky_todolist.git
cd dicky_todolist
dotnet restore

```

### 2. Setup Database

Proyek menggunakan SQLite, jalankan migrasi untuk membuat file `todo.db`:

```bash
dotnet ef database update

```

### 3. Jalankan Aplikasi

```bash
dotnet run

```

Aplikasi berjalan di: `http://localhost:5170`

---

## 📑 API Endpoints

### Auth Endpoints

| Method   | Endpoint             | Deskripsi                       |
| -------- | -------------------- | ------------------------------- |
| **POST** | `/api/auth/register` | Mendaftarkan user baru          |
| **POST** | `/api/auth/login`    | Login dan mendapatkan JWT Token |

### Todo Endpoints (Membutuhkan Bearer Token)

| Method     | Endpoint                   | Deskripsi                                     |
| ---------- | -------------------------- | --------------------------------------------- |
| **GET**    | `/api/todos`               | Mendapatkan semua daftar todo (Milik sendiri) |
| **GET**    | `/api/todos/{id}`          | Mendapatkan detail todo berdasarkan ID        |
| **POST**   | `/api/todos`               | Membuat todo baru                             |
| **PUT**    | `/api/todos/{id}`          | Update data todo                              |
| **PATCH**  | `/api/todos/{id}/complete` | Menandai todo selesai                         |
| **DELETE** | `/api/todos/{id}`          | Soft delete todo                              |

---

## 🧪 Cara Pengujian (Swagger)

1. Buka `http://localhost:5170/swagger`.
2. Gunakan endpoint `/api/auth/register` untuk membuat akun.
3. Login melalui `/api/auth/login` untuk mendapatkan token.
4. Klik tombol **"Authorize"** di pojok kanan atas Swagger.
5. Masukkan token dengan format: `Bearer {token_kamu}`.
6. Sekarang kamu bisa mengakses endpoint Todo.

## ⚙️ Unit Testing

Jalankan perintah berikut di folder test:

```bash
dotnet test

```

---
