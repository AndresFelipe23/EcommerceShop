import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import type { Usuario, LoginRequest } from '../types/auth';
import { apiService } from '../services/api';
import { config } from '../config';

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
    const savedUsuario = localStorage.getItem(config.auth.userKey);
    const token = localStorage.getItem(config.auth.tokenKey);
    
    if (savedUsuario && token) {
      try {
        setUsuario(JSON.parse(savedUsuario));
      } catch (error) {
        console.error('Error al parsear usuario guardado:', error);
        localStorage.removeItem(config.auth.userKey);
        localStorage.removeItem(config.auth.tokenKey);
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
      localStorage.setItem(config.auth.tokenKey, response.token);
      localStorage.setItem(config.auth.userKey, JSON.stringify(response.usuario));
      
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
      localStorage.removeItem(config.auth.tokenKey);
      localStorage.removeItem(config.auth.userKey);
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