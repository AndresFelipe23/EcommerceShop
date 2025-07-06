// Tipos de autenticación
export interface Usuario {
  id: number;
  nombre: string;
  apellido: string;
  email: string;
  password?: string;
  telefono?: string;
  fechaNacimiento?: string;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  usuario: Usuario;
} 