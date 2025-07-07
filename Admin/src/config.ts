// Configuración del frontend
export const config = {
  // URL de la API del backend
  apiUrl: import.meta.env.VITE_API_URL || 'http://localhost:5268/api',
  
  // Configuración de la aplicación
  app: {
    name: 'Admin Ecommerce',
    version: '1.0.0'
  },
  
  // Configuración de autenticación
  auth: {
    tokenKey: 'token',
    userKey: 'usuario',
    refreshTokenKey: 'refreshToken'
  }
}; 