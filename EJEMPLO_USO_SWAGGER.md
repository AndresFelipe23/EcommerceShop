# Guía de Uso de Swagger con JWT

## 🚀 Cómo usar Swagger con autenticación JWT

### 1. **Acceder a Swagger**
- Ejecuta la aplicación
- Ve a: `https://localhost:7000/swagger` (o el puerto que uses)
- Verás la interfaz de Swagger con todos los endpoints

### 2. **Registrar un nuevo usuario**
1. Busca el endpoint `POST /api/auth/register`
2. Haz clic en "Try it out"
3. Ingresa los datos de ejemplo:
```json
{
  "usuNombre": "Juan",
  "usuApellido": "Pérez",
  "usuEmail": "juan.perez@email.com",
  "usuPassword": "123456",
  "usuTelefono": "3001234567",
  "usuGenero": "Masculino",
  "usuFechaNacimiento": "1990-01-01"
}
```
4. Haz clic en "Execute"
5. **Copia el token** de la respuesta

### 3. **Autenticarse (Login)**
1. Busca el endpoint `POST /api/auth/login`
2. Haz clic en "Try it out"
3. Ingresa los datos:
```json
{
  "email": "juan.perez@email.com",
  "password": "123456"
}
```
4. Haz clic en "Execute"
5. **Copia el token** de la respuesta

### 4. **Usar el token en endpoints protegidos**
1. En la parte superior de Swagger, verás un botón **"Authorize"**
2. Haz clic en él
3. En el campo "Value", ingresa: `Bearer TU_TOKEN_AQUI`
   - Ejemplo: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
4. Haz clic en "Authorize"
5. Cierra el modal

### 5. **Probar endpoints protegidos**
Ahora puedes probar endpoints como:
- `GET /api/auth/me` - Obtener usuario actual
- `GET /api/auth/test-auth` - Probar autenticación
- `GET /api/usuario` - Listar usuarios (protegido)
- `GET /api/usuario/{id}` - Obtener usuario específico

### 6. **Renovar token**
Si el token expira:
1. Usa el endpoint `POST /api/auth/refresh-token`
2. Envía el token actual:
```json
{
  "token": "TU_TOKEN_ACTUAL"
}
```
3. Obtén un nuevo token

## 🔧 Endpoints Disponibles

### **Sin Autenticación:**
- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Registro
- `POST /api/auth/refresh-token` - Renovar token

### **Con Autenticación:**
- `GET /api/auth/me` - Usuario actual
- `GET /api/auth/test-auth` - Probar auth
- `POST /api/auth/logout` - Logout
- `GET /api/usuario` - Listar usuarios
- `GET /api/usuario/{id}` - Obtener usuario
- `PUT /api/usuario/{id}` - Actualizar usuario
- `DELETE /api/usuario/{id}` - Eliminar usuario

## 📝 Ejemplos de Respuestas

### **Login Exitoso:**
```json
{
  "exito": true,
  "mensaje": "Login exitoso",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abc123...",
  "usuario": {
    "usuId": 1,
    "usuNombre": "Juan",
    "usuApellido": "Pérez",
    "usuEmail": "juan.perez@email.com",
    "usuEstadoCuenta": "Activo",
    "usuFechaRegistro": "2024-01-01T00:00:00",
    "nombreCompleto": "Juan Pérez"
  }
}
```

### **Error de Autenticación:**
```json
{
  "mensaje": "Credenciales inválidas"
}
```

## ⚠️ Notas Importantes

1. **El token expira en 60 minutos** (configurable en appsettings.json)
2. **Usa siempre el formato**: `Bearer TOKEN`
3. **Los refresh tokens** permiten renovar sin volver a hacer login
4. **Guarda el token** después del login/registro
5. **Si ves error 401**, renueva el token

## 🛠️ Solución de Problemas

### **Error 401 Unauthorized:**
- Verifica que el token esté en el formato correcto
- Asegúrate de que el token no haya expirado
- Usa el endpoint de refresh-token

### **Error 400 Bad Request:**
- Verifica el formato JSON de la petición
- Asegúrate de que todos los campos requeridos estén presentes

### **Error 500 Internal Server Error:**
- Revisa los logs de la aplicación
- Verifica la conexión a la base de datos 