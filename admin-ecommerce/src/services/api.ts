import axios from 'axios';
import type { AxiosInstance, AxiosResponse } from 'axios';
import type { 
  LoginRequest, 
  LoginResponse, 
  Usuario
} from '../types/auth';
import type { 
  Producto, 
  Categoria, 
  Talla, 
  Color, 
  Temporada,
  Inventario,
  Pedido,
  Promocion,
  Cupon,
  ApiResponse
} from '../types';

class ApiService {
  private api: AxiosInstance;
  private baseURL = import.meta.env.VITE_API_URL || 'http://localhost:5268/api'; // URL de tu API

  constructor() {
    this.api = axios.create({
      baseURL: this.baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Interceptor para agregar el token de autenticación
    this.api.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('token');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    // Interceptor para manejar errores de respuesta
    this.api.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          localStorage.removeItem('token');
          localStorage.removeItem('usuario');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // Autenticación
  async login(credentials: LoginRequest): Promise<LoginResponse> {
    const response: AxiosResponse<ApiResponse<LoginResponse>> = await this.api.post('/Auth/login', credentials);
    return response.data.data!;
  }

  async logout(): Promise<void> {
    await this.api.post('/Auth/logout');
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
  }

  // Usuarios
  async getUsuarios(): Promise<Usuario[]> {
    try {
      const response: AxiosResponse<ApiResponse<Usuario[]>> = await this.api.get('/Usuario');
      return response.data.data || [];
    } catch (error) {
      console.warn('Error al obtener usuarios:', error);
      return [];
    }
  }

  async getUsuario(id: number): Promise<Usuario> {
    const response: AxiosResponse<ApiResponse<Usuario>> = await this.api.get(`/Usuario/${id}`);
    return response.data.data!;
  }

  async createUsuario(usuario: Partial<Usuario>): Promise<Usuario> {
    const response: AxiosResponse<ApiResponse<Usuario>> = await this.api.post('/Usuario', usuario);
    return response.data.data!;
  }

  async updateUsuario(id: number, usuario: Partial<Usuario>): Promise<Usuario> {
    const response: AxiosResponse<ApiResponse<Usuario>> = await this.api.put(`/Usuario/${id}`, usuario);
    return response.data.data!;
  }

  async deleteUsuario(id: number): Promise<void> {
    await this.api.delete(`/Usuario/${id}`);
  }

  // Productos
  async getProductos(): Promise<Producto[]> {
    try {
      const response: AxiosResponse<ApiResponse<Producto[]>> = await this.api.get('/Producto');
      return response.data.data || [];
    } catch (error) {
      console.warn('Error al obtener productos:', error);
      return [];
    }
  }

  async getProducto(id: number): Promise<Producto> {
    const response: AxiosResponse<ApiResponse<Producto>> = await this.api.get(`/Producto/${id}`);
    return response.data.data!;
  }

  async createProducto(producto: Partial<Producto>): Promise<Producto> {
    const response: AxiosResponse<ApiResponse<Producto>> = await this.api.post('/Producto', producto);
    return response.data.data!;
  }

  async updateProducto(id: number, producto: Partial<Producto>): Promise<Producto> {
    const response: AxiosResponse<ApiResponse<Producto>> = await this.api.put(`/Producto/${id}`, producto);
    return response.data.data!;
  }

  async deleteProducto(id: number): Promise<void> {
    await this.api.delete(`/Producto/${id}`);
  }

  // Categorías
  async getCategorias(): Promise<Categoria[]> {
    const response: AxiosResponse<ApiResponse<Categoria[]>> = await this.api.get('/Categoria');
    return response.data.data!;
  }

  async createCategoria(categoria: Partial<Categoria>): Promise<Categoria> {
    const response: AxiosResponse<ApiResponse<Categoria>> = await this.api.post('/Categoria', categoria);
    return response.data.data!;
  }

  async updateCategoria(id: number, categoria: Partial<Categoria>): Promise<Categoria> {
    const response: AxiosResponse<ApiResponse<Categoria>> = await this.api.put(`/Categoria/${id}`, categoria);
    return response.data.data!;
  }

  async deleteCategoria(id: number): Promise<void> {
    await this.api.delete(`/Categoria/${id}`);
  }

  // Tallas
  async getTallas(): Promise<Talla[]> {
    try {
      const response: AxiosResponse<Talla[]> = await this.api.get('/Talla');
      console.log('Respuesta del servidor para tallas:', response.data);
      return response.data || [];
    } catch (error) {
      console.error('Error detallado al obtener tallas:', error);
      if (error.response) {
        console.error('Respuesta del servidor:', error.response.data);
        console.error('Status:', error.response.status);
      }
      return [];
    }
  }

  async getTalla(id: number): Promise<Talla> {
    const response: AxiosResponse<Talla> = await this.api.get(`/Talla/${id}`);
    return response.data;
  }

  async createTalla(talla: Partial<Talla>): Promise<Talla> {
    const response: AxiosResponse<Talla> = await this.api.post('/Talla', talla);
    return response.data;
  }

  async updateTalla(id: number, talla: Partial<Talla>): Promise<Talla> {
    const response: AxiosResponse<Talla> = await this.api.put(`/Talla/${id}`, talla);
    return response.data;
  }

  async deleteTalla(id: number): Promise<void> {
    await this.api.delete(`/Talla/${id}`);
  }

  // Colores
  async getColores(): Promise<Color[]> {
    const response: AxiosResponse<ApiResponse<Color[]>> = await this.api.get('/Color');
    return response.data.data!;
  }

  async createColor(color: Partial<Color>): Promise<Color> {
    const response: AxiosResponse<ApiResponse<Color>> = await this.api.post('/Color', color);
    return response.data.data!;
  }

  async updateColor(id: number, color: Partial<Color>): Promise<Color> {
    const response: AxiosResponse<ApiResponse<Color>> = await this.api.put(`/Color/${id}`, color);
    return response.data.data!;
  }

  async deleteColor(id: number): Promise<void> {
    await this.api.delete(`/Color/${id}`);
  }

  // Temporadas
  async getTemporadas(): Promise<Temporada[]> {
    const response: AxiosResponse<ApiResponse<Temporada[]>> = await this.api.get('/Temporada');
    return response.data.data!;
  }

  async createTemporada(temporada: Partial<Temporada>): Promise<Temporada> {
    const response: AxiosResponse<ApiResponse<Temporada>> = await this.api.post('/Temporada', temporada);
    return response.data.data!;
  }

  async updateTemporada(id: number, temporada: Partial<Temporada>): Promise<Temporada> {
    const response: AxiosResponse<ApiResponse<Temporada>> = await this.api.put(`/Temporada/${id}`, temporada);
    return response.data.data!;
  }

  async deleteTemporada(id: number): Promise<void> {
    await this.api.delete(`/Temporada/${id}`);
  }

  // Inventario
  async getInventario(): Promise<Inventario[]> {
    const response: AxiosResponse<ApiResponse<Inventario[]>> = await this.api.get('/Inventario');
    return response.data.data!;
  }

  async createInventario(inventario: Partial<Inventario>): Promise<Inventario> {
    const response: AxiosResponse<ApiResponse<Inventario>> = await this.api.post('/Inventario', inventario);
    return response.data.data!;
  }

  async updateInventario(id: number, inventario: Partial<Inventario>): Promise<Inventario> {
    const response: AxiosResponse<ApiResponse<Inventario>> = await this.api.put(`/Inventario/${id}`, inventario);
    return response.data.data!;
  }

  async deleteInventario(id: number): Promise<void> {
    await this.api.delete(`/Inventario/${id}`);
  }

  // Pedidos
  async getPedidos(): Promise<Pedido[]> {
    try {
      const response: AxiosResponse<ApiResponse<Pedido[]>> = await this.api.get('/Pedido');
      return response.data.data || [];
    } catch (error) {
      console.warn('Error al obtener pedidos:', error);
      return [];
    }
  }

  async getPedido(id: number): Promise<Pedido> {
    const response: AxiosResponse<ApiResponse<Pedido>> = await this.api.get(`/Pedido/${id}`);
    return response.data.data!;
  }

  async updatePedido(id: number, pedido: Partial<Pedido>): Promise<Pedido> {
    const response: AxiosResponse<ApiResponse<Pedido>> = await this.api.put(`/Pedido/${id}`, pedido);
    return response.data.data!;
  }

  // Promociones
  async getPromociones(): Promise<Promocion[]> {
    const response: AxiosResponse<ApiResponse<Promocion[]>> = await this.api.get('/Promocion');
    return response.data.data!;
  }

  async createPromocion(promocion: Partial<Promocion>): Promise<Promocion> {
    const response: AxiosResponse<ApiResponse<Promocion>> = await this.api.post('/Promocion', promocion);
    return response.data.data!;
  }

  async updatePromocion(id: number, promocion: Partial<Promocion>): Promise<Promocion> {
    const response: AxiosResponse<ApiResponse<Promocion>> = await this.api.put(`/Promocion/${id}`, promocion);
    return response.data.data!;
  }

  async deletePromocion(id: number): Promise<void> {
    await this.api.delete(`/Promocion/${id}`);
  }

  // Cupones
  async getCupones(): Promise<Cupon[]> {
    const response: AxiosResponse<ApiResponse<Cupon[]>> = await this.api.get('/Cupon');
    return response.data.data!;
  }

  async createCupon(cupon: Partial<Cupon>): Promise<Cupon> {
    const response: AxiosResponse<ApiResponse<Cupon>> = await this.api.post('/Cupon', cupon);
    return response.data.data!;
  }

  async updateCupon(id: number, cupon: Partial<Cupon>): Promise<Cupon> {
    const response: AxiosResponse<ApiResponse<Cupon>> = await this.api.put(`/Cupon/${id}`, cupon);
    return response.data.data!;
  }

  async deleteCupon(id: number): Promise<void> {
    await this.api.delete(`/Cupon/${id}`);
  }

  // Método de prueba para verificar conectividad
  async testConnection(): Promise<boolean> {
    try {
      const response = await this.api.get('/Usuario');
      return response.status === 200;
    } catch (error) {
      console.error('Error de conexión al servidor:', error);
      return false;
    }
  }
}

export const apiService = new ApiService(); 