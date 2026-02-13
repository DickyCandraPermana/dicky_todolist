# dicky_todolist

Proyek ini adalah REST API sederhana untuk mengelola **Todo List** yang dibangun menggunakan **ASP.NET Core 8** menggunakan database **SQLite**. Proyek ini dibuat untuk memenuhi Technical Test Backend Intern.

---

## Teknologi & Versi

- **Framework:** .NET 8
- **Database:** SQLite (Entity Framework Core)
- **Tools:** Swagger UI (Swashbuckle)

---

## Cara Menjalankan Project

### 1. Clone Repository

```bash
git clone https://github.com/DickyCandraPermana/dicky_todolist.git
cd dicky_todolist
```

### 2. Download Dependency

Jalankan perintah berikut untuk mengunduh library yang dibutuhkan:

```bash
dotnet restore
```

### 3. Setup Database (Migration)

Proyek ini menggunakan SQLite. Jalankan migrasi untuk membuat database lokal:

```bash
# Pastikan sudah install dotnet-ef secara global
dotnet ef database update
```

File todo.db akan terbuat secara otomatis di root folder.

### 4. Jalankan Aplikasi

```bash
dotnet run
```

Aplikasi akan berjalan di: https://localhost:5170

---

## API Endpoints

| Method     | Endpoints                 | Deskripsi                            |
| :--------- | :------------------------ | :----------------------------------- |
| **GET**    | `api/todos`               | Mendapatkan semua daftar todo        |
| **GET**    | `api/todos/{id}`          | Mendapatkan todo berdasarkan ID      |
| **POST**   | `api/todos`               | Membuat todo baru                    |
| **PUT**    | `api/todos/{id}`          | Mengedit data todo                   |
| **PUT**    | `api/todos/{id}/complete` | Mengubah status todo menjadi selesai |
| **DELETE** | `api/todos/{id}`          | Menghapus todo berdasarkan ID        |

### Contoh Request Body (Tambah Todo)

```json
{
  "title": "Take a note guys"
}
```

---

## Struktur Folders

- `/Controllers` : Logic untuk handling request dan response.
- `/Models` : Definisi entity Todo (Guid Id, string Title, string Description, bool IsCompleted, DateTime CreatedAt).
- `/Data` : Konfigurasi AppDbContext untuk akses SQLite.
- `/DTO` : Logic untuk validasi request dan response.

---

## Testing

Setelah aplikasi berjalan, buka browser dan akses:
[`https://localhost:5170/swagger`](https://localhost:5170/swagger)
untuk mencoba endpoint secara langsung.

## Unit Test

Unit test bisa dijalankan dengan mengakses ToDoList.Tests

### 1. Akses Unit Test

```bash
cd ToDoList.Tests
```

### 2. Download Dependency

```bash
dotnet restore
```

### 3. Jalankan Unit Test

```bash
dotnet test
```