# JwtAuthApi

A secure ASP.NET Core Web API project implementing **custom JWT authentication** using Access & Refresh tokens — without ASP.NET Core Identity. Includes full user registration, login, secure password hashing, and token issuance.

---

## 🔧 Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core
- JWT Authentication
- SQL Server (LocalDB or full SQL)
- Swagger for testing
- Custom DTOs (not exposing entities)

---

## 🚀 Features

- ✅ Secure user registration with hashed passwords
- ✅ Login and receive JWT **access** and **refresh** tokens
- ✅ Refresh access tokens using a valid refresh token
- ✅ Role claim included in access token (for future use)
- ✅ Clean architecture with DTOs
- ✅ EF Core migrations
- ✅ Swagger UI integration

---

## 📁 Folder Structure

```
JwtAuthApi/
├── Controllers/
│   └── AuthController.cs
│   └── SecureController.cs
├── Models/
│   └── User.cs
│   └── RefreshToken.cs
│   └── RegisterDto.cs
│   └── LoginDto.cs
├── Services/
│   └── TokenService.cs
├── Data/
│   └── ApplicationDbContext.cs
├── appsettings.json
├── Program.cs
```

---

## 🧱 Database Setup

1. Modify the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JwtAuthDb;Trusted_Connection=True;"
}
```

2. Add EF Core tools:
```bash
Install-Package Microsoft.EntityFrameworkCore.Tools
```

3. Create migration and update database:
```powershell
Add-Migration Init
Update-Database
```

---

## 🔐 Authentication Flow

### 1. Registration

**Endpoint:** `POST /api/auth/register`  
**Request Body:**
```json
{
  "name": "YourName",
  "surname": "YourSurname",
  "email": "example@email.com",
  "password": "YourStrongPassword"
}
```

Creates a new user with hashed password and default `"User"` role.

---

### 2. Login

**Endpoint:** `POST /api/auth/login`  
**Request Body:**
```json
{
  "email": "example@email.com",
  "password": "YourStrongPassword"
}
```

Returns:
```json
{
  "token": "<JWT access token>",
  "refreshToken": "<refresh token>"
}
```

---

### 3. Refresh Token

**Endpoint:** `POST /api/auth/refresh`  
**Request Body (raw string):**
```text
<refreshToken>
```

Returns a new access token + refresh token pair.

---

## 🛡️ JWT Token Claims

The issued access token contains:

- `sub` / `nameid`: User ID
- `name`: Name
- `surname` : Surname
- `email`: Email address
- `role`: Role (e.g., User, Admin)

---

## 🧪 Testing with Swagger

1. Run the project.
2. Go to Swagger: `https://localhost:<port>/swagger`
3. Use `/register` and `/login` to get a JWT.
4. Click **Authorize** and paste:  
   `Bearer <access_token>`
5. Call any protected endpoint.

---

## 💡 Notes

- DTOs (`RegisterDto`, `LoginDto`) are **not stored in the DB**.
- Only `User` and `RefreshToken` are real EF Core entities.
- You can freely customize roles, user properties, and token logic.

---

## 📌 Next Steps

- Add `[Authorize]` attributes to secure endpoints
- Add audit logging
- Support role-based access (`Admin`, `User`, etc.)