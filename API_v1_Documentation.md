# 🚀 SportyBackend API v1 - REST Endpoints

## 📋 **Endpoints Overview**

Esta nueva versión de la API sigue los principios REST y está organizada en módulos funcionales para mejor mantenibilidad y escalabilidad.

### **Base URL**: `api/v1`

---

## 🔐 **Authentication** (`/auth`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/auth/login` | User authentication |
| POST | `/auth/refresh` | Refresh access token |
| POST | `/auth/logout` | Logout user |
| POST | `/auth/logout-all` | Logout from all devices |

---

## 👥 **Users Management** (`/users`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/users` | Get all users |
| POST | `/users` | Create new user |
| GET | `/users/{id}` | Get user by ID |

### **User Roles** (`/users/{userId}/roles`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| PUT | `/users/{userId}/roles` | Assign role to user |

### **User Sports** (`/users/{userId}/sports`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/users/{userId}/sports` | Get user sports |
| POST | `/users/{userId}/sports` | Assign sport to user |
| DELETE | `/users/{userId}/sports/{sportId}` | Remove sport from user |
| PUT | `/users/{userId}/sports/{sportId}/configuration` | Configure user sport |

---

## ⚽ **Sports Management** (`/sports`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/sports` | Get all sports | ✅ `administrar_deportes` |
| POST | `/sports` | Create new sport | ✅ `administrar_deportes` |
| GET | `/sports/{id}` | Get sport by ID | ✅ `administrar_deportes` |
| PUT | `/sports/{id}` | Update sport | ✅ `administrar_deportes` |
| DELETE | `/sports/{id}` | Delete sport | ✅ `administrar_deportes` |

---

## 🌍 **Locations Management** 

### **Countries** (`/countries`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/countries` | Create country | ✅ `administrar_parametros` |
| GET | `/countries/{id}/hierarchy` | Get location hierarchy | ❌ |
| GET | `/countries/{id}/document-types` | Get document types | ❌ |
| POST | `/countries/{id}/document-types` | Create document type | ✅ `administrar_parametros` |

### **States/Departments** (`/countries/{countryId}/states`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/countries/{countryId}/states` | Create state/department | ✅ `administrar_parametros` |

### **Cities** (`/states/{stateId}/cities`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/states/{stateId}/cities` | Create municipality | ✅ `administrar_parametros` |
| POST | `/states/{stateId}/cities/localities` | Create locality | ✅ `administrar_parametros` |
| POST | `/states/{stateId}/cities/alternative` | Create city (alternative) | ✅ `administrar_parametros` |

---

## 🔒 **Security Management**

### **Roles** (`/roles`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/roles` | Get all roles |
| POST | `/roles` | Create new role |
| GET | `/roles/{id}` | Get role by ID |
| PUT | `/roles/{id}` | Update role |
| DELETE | `/roles/{id}` | Delete role |

### **Permissions** (`/permissions`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/permissions` | Get all permissions |
| POST | `/permissions` | Create new permission |
| GET | `/permissions/{id}` | Get permission by ID |
| PUT | `/permissions/{id}` | Update permission |
| DELETE | `/permissions/{id}` | Delete permission |

### **Role Permissions** (`/roles/{roleId}/permissions`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/roles/{roleId}/permissions` | Get role permissions |
| PUT | `/roles/{roleId}/permissions` | Assign permissions to role |
| DELETE | `/roles/{roleId}/permissions/{permissionId}` | Remove specific permission |
| POST | `/roles/{roleId}/permissions/remove` | Remove multiple permissions |

---

## 👑 **Admin Management** (`/admin`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/admin/users` | Create owner/admin user |
| POST | `/admin/users/login` | Owner user login |

---

## 🎯 **Key Improvements**

### **✅ REST Compliance**
- **Resource-based URLs**: `/users`, `/sports`, `/roles`
- **HTTP methods semantics**: GET, POST, PUT, DELETE
- **Nested resources**: `/users/{id}/sports`
- **Consistent naming**: English, plural nouns

### **✅ Better Organization**
- **Modular structure**: Auth, Users, Sports, Locations, Security, Admin
- **Logical grouping**: Related functionality together
- **Clear hierarchy**: Parent-child relationships

### **✅ Enhanced Maintainability**
- **Single responsibility**: Each controller has a focused purpose
- **Consistent error handling**: Unified response format
- **Better documentation**: Clear endpoint descriptions

### **✅ Improved Scalability**
- **Versioned API**: `/v1/` prefix for future versions
- **Granular permissions**: Endpoint-level authorization
- **Modular expansion**: Easy to add new modules

---

## 🚦 **Migration Strategy**

1. **Phase 1**: Deploy new v1 controllers alongside existing ones
2. **Phase 2**: Update client applications to use v1 endpoints
3. **Phase 3**: Deprecate old endpoints with proper versioning
4. **Phase 4**: Remove legacy controllers

---

## 📖 **Usage Examples**

### **Create User**
```http
POST /api/v1/users
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "password123",
  "documentTypeId": "doc-type-id",
  "documentNumber": "12345678",
  "countryId": "country-id",
  "departmentId": "department-id",
  "municipalityId": "municipality-id"
}
```

### **Assign Sport to User**
```http
POST /api/v1/users/user-123/sports
Content-Type: application/json

{
  "sportId": "sport-456"
}
```

### **Get User Sports**
```http
GET /api/v1/users/user-123/sports
Authorization: Bearer {token}
```

### **Assign Permissions to Role**
```http
PUT /api/v1/roles/role-123/permissions
Content-Type: application/json

{
  "permissionIds": ["perm-1", "perm-2", "perm-3"]
}
```

---

## 🔧 **Response Format**

### **Success Response**
```json
{
  "data": {...},
  "message": "Operation successful"
}
```

### **Error Response**
```json
{
  "message": "Error description"
}
```

---

## 🛡️ **Security**

- **JWT Authentication**: Bearer token required for protected endpoints
- **Permission-based Authorization**: Granular access control
- **Rate Limiting**: API rate limiting middleware
- **Input Sanitization**: Request validation and sanitization
