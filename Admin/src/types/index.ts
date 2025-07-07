// Tipos de productos
export interface Producto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  categoriaId: number;
  categoria?: Categoria;
  temporadaId: number;
  temporada?: Temporada;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

export interface Categoria {
  id: number;
  nombre: string;
  descripcion?: string;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

export interface Talla {
  tallaId: number;
  talNombre: string;
  talGenero: string;
  talOrdenVisualizacion?: number | null;
}

export interface Color {
  id: number;
  nombre: string;
  codigoHex?: string;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

export interface Temporada {
  id: number;
  nombre: string;
  descripcion?: string;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

// Tipos de inventario
export interface Inventario {
  id: number;
  productoId: number;
  producto?: Producto;
  tallaId: number;
  talla?: Talla;
  colorId: number;
  color?: Color;
  cantidad: number;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

// Tipos de pedidos
export interface Pedido {
  id: number;
  usuarioId: number;
  usuario?: Usuario;
  estado: string;
  total: number;
  fechaCreacion: string;
  fechaActualizacion?: string;
  detalles?: PedidoDetalle[];
}

export interface PedidoDetalle {
  id: number;
  pedidoId: number;
  productoId: number;
  producto?: Producto;
  cantidad: number;
  precioUnitario: number;
  subtotal: number;
  fechaCreacion: string;
}

// Tipos de promociones
export interface Promocion {
  id: number;
  nombre: string;
  descripcion: string;
  descuento: number;
  fechaInicio: string;
  fechaFin: string;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

// Tipos de cupones
export interface Cupon {
  id: number;
  codigo: string;
  descripcion: string;
  descuento: number;
  fechaInicio: string;
  fechaFin: string;
  activo: boolean;
  fechaCreacion: string;
  fechaActualizacion?: string;
}

// Tipos de usuarios
export interface Usuario {
  usuId: number;
  usuNombre: string;
  usuApellido: string;
  usuEmail: string;
  usuTelefono?: string;
  usuGenero?: string;
  usuFechaNacimiento?: string;
  usuImagenPerfil?: string;
  usuEstadoCuenta: string;
  usuProveedorAutenticacion?: string;
  usuFechaRegistro: string;
  usuFechaActualizacion?: string;
  activo: boolean;
}

// Re-exportar tipos de API
export * from './api'; 