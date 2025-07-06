import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import type { Usuario, LoginRequest } from '../types/auth';
import { apiService } from '../services/api';

interface AuthContextType {
  usuario: Usuario | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (credentials: LoginRequest) => Promise<void>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth debe ser usado dentro de un AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [usuario, setUsuario] = useState<Usuario | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    // Verificar si hay un usuario guardado en localStorage
    const savedUsuario = localStorage.getItem('usuario');
    const token = localStorage.getItem('token');
    
    if (savedUsuario && token) {
      try {
        setUsuario(JSON.parse(savedUsuario));
      } catch (error) {
        console.error('Error al parsear usuario guardado:', error);
        localStorage.removeItem('usuario');
        localStorage.removeItem('token');
      }
    }
    
    setIsLoading(false);
  }, []);

  const login = async (credentials: LoginRequest) => {
    try {
      setIsLoading(true);
      const response = await apiService.login(credentials);
      
      // Verificar que la respuesta tenga los datos necesarios
      if (!response || !response.token || !response.usuario) {
        throw new Error('Respuesta de autenticación inválida');
      }
      
      // Guardar token y usuario en localStorage
      localStorage.setItem('token', response.token);
      localStorage.setItem('usuario', JSON.stringify(response.usuario));
      
      setUsuario(response.usuario);
    } catch (error) {
      console.error('Error en login:', error);
      throw error;
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    try {
      await apiService.logout();
    } catch (error) {
      console.error('Error en logout:', error);
    } finally {
      setUsuario(null);
      localStorage.removeItem('token');
      localStorage.removeItem('usuario');
    }
  };

  const value: AuthContextType = {
    usuario,
    isAuthenticated: !!usuario,
    isLoading,
    login,
    logout,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}; 