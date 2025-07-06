import React, { useState, useEffect } from 'react';
import { 
  Package, 
  Users, 
  ShoppingCart, 
  DollarSign,
  TrendingUp,
  TrendingDown,
  Eye,
  ShoppingBag
} from 'lucide-react';
import { apiService } from '../services/api';
import type { Producto, Pedido } from '../types';
import type { Usuario } from '../types/auth';
import type { DashboardStats } from '../types/dashboard';

const Dashboard: React.FC = () => {
  const [stats, setStats] = useState<DashboardStats>({
    totalProductos: 0,
    totalUsuarios: 0,
    totalPedidos: 0,
    totalVentas: 0,
    productosActivos: 0,
    usuariosActivos: 0,
    pedidosPendientes: 0,
    pedidosCompletados: 0,
  });
  const [recentProducts, setRecentProducts] = useState<Producto[]>([]);
  const [recentOrders, setRecentOrders] = useState<Pedido[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setIsLoading(true);
        
        // Probar conectividad primero
        const isConnected = await apiService.testConnection();
        if (!isConnected) {
          console.warn('No se puede conectar al servidor');
          // Establecer valores por defecto
          setStats({
            totalProductos: 0,
            totalUsuarios: 0,
            totalPedidos: 0,
            totalVentas: 0,
            productosActivos: 0,
            usuariosActivos: 0,
            pedidosPendientes: 0,
            pedidosCompletados: 0,
          });
          setRecentProducts([]);
          setRecentOrders([]);
          return;
        }
        
        // Obtener datos - los métodos del API ya manejan errores y devuelven arrays vacíos
        const [productos, usuarios, pedidos] = await Promise.all([
          apiService.getProductos(),
          apiService.getUsuarios(),
          apiService.getPedidos(),
        ]);

        // Calcular estadísticas
        const totalVentas = pedidos.reduce((sum, pedido) => sum + (pedido.total || 0), 0);
        const productosActivos = productos.filter(p => p.activo).length;
        const usuariosActivos = usuarios.filter(u => u.activo).length;
        const pedidosPendientes = pedidos.filter(p => p.estado === 'Pendiente').length;
        const pedidosCompletados = pedidos.filter(p => p.estado === 'Completado').length;

        setStats({
          totalProductos: productos.length,
          totalUsuarios: usuarios.length,
          totalPedidos: pedidos.length,
          totalVentas,
          productosActivos,
          usuariosActivos,
          pedidosPendientes,
          pedidosCompletados,
        });

        // Productos recientes (últimos 5)
        setRecentProducts(productos.slice(0, 5));
        
        // Pedidos recientes (últimos 5)
        setRecentOrders(pedidos.slice(0, 5));

      } catch (error) {
        console.error('Error al cargar datos del dashboard:', error);
        // Establecer valores por defecto en caso de error
        setStats({
          totalProductos: 0,
          totalUsuarios: 0,
          totalPedidos: 0,
          totalVentas: 0,
          productosActivos: 0,
          usuariosActivos: 0,
          pedidosPendientes: 0,
          pedidosCompletados: 0,
        });
        setRecentProducts([]);
        setRecentOrders([]);
      } finally {
        setIsLoading(false);
      }
    };

    fetchDashboardData();
  }, []);

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
    }).format(amount);
  };

  const StatCard = ({ 
    title, 
    value, 
    icon: Icon, 
    trend, 
    trendValue, 
    color = 'blue' 
  }: {
    title: string;
    value: string | number;
    icon: any;
    trend?: 'up' | 'down';
    trendValue?: string;
    color?: string;
  }) => (
    <div className="bg-white overflow-hidden shadow rounded-lg">
      <div className="p-5">
        <div className="flex items-center">
          <div className="flex-shrink-0">
            <Icon className={`h-6 w-6 text-${color}-600`} />
          </div>
          <div className="ml-5 w-0 flex-1">
            <dl>
              <dt className="text-sm font-medium text-gray-500 truncate">
                {title}
              </dt>
              <dd className="text-lg font-medium text-gray-900">
                {value}
              </dd>
            </dl>
          </div>
        </div>
        {trend && (
          <div className="mt-4">
            <div className={`flex items-center text-sm ${
              trend === 'up' ? 'text-green-600' : 'text-red-600'
            }`}>
              {trend === 'up' ? (
                <TrendingUp className="h-4 w-4 mr-1" />
              ) : (
                <TrendingDown className="h-4 w-4 mr-1" />
              )}
              {trendValue}
            </div>
          </div>
        )}
      </div>
    </div>
  );

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Dashboard</h1>
        <p className="mt-1 text-sm text-gray-500">
          Resumen general del ecommerce
        </p>
      </div>

      {/* Estadísticas principales */}
      <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
        <StatCard
          title="Total Productos"
          value={stats.totalProductos}
          icon={Package}
          color="blue"
        />
        <StatCard
          title="Total Usuarios"
          value={stats.totalUsuarios}
          icon={Users}
          color="green"
        />
        <StatCard
          title="Total Pedidos"
          value={stats.totalPedidos}
          icon={ShoppingCart}
          color="purple"
        />
        <StatCard
          title="Total Ventas"
          value={formatCurrency(stats.totalVentas)}
          icon={DollarSign}
          color="yellow"
        />
      </div>

      {/* Estadísticas secundarias */}
      <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4">
        <StatCard
          title="Productos Activos"
          value={stats.productosActivos}
          icon={ShoppingBag}
          color="blue"
        />
        <StatCard
          title="Usuarios Activos"
          value={stats.usuariosActivos}
          icon={Users}
          color="green"
        />
        <StatCard
          title="Pedidos Pendientes"
          value={stats.pedidosPendientes}
          icon={ShoppingCart}
          color="orange"
        />
        <StatCard
          title="Pedidos Completados"
          value={stats.pedidosCompletados}
          icon={ShoppingCart}
          color="green"
        />
      </div>

      {/* Contenido reciente */}
      <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
        {/* Productos recientes */}
        <div className="bg-white shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
              Productos Recientes
            </h3>
            <div className="space-y-3">
              {recentProducts.map((producto) => (
                <div key={producto.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                  <div className="flex items-center">
                    <Package className="h-5 w-5 text-gray-400 mr-3" />
                    <div>
                      <p className="text-sm font-medium text-gray-900">{producto.nombre}</p>
                      <p className="text-sm text-gray-500">{formatCurrency(producto.precio)}</p>
                    </div>
                  </div>
                  <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                    producto.activo 
                      ? 'bg-green-100 text-green-800' 
                      : 'bg-red-100 text-red-800'
                  }`}>
                    {producto.activo ? 'Activo' : 'Inactivo'}
                  </span>
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* Pedidos recientes */}
        <div className="bg-white shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
              Pedidos Recientes
            </h3>
            <div className="space-y-3">
              {recentOrders.map((pedido) => (
                <div key={pedido.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                  <div className="flex items-center">
                    <ShoppingCart className="h-5 w-5 text-gray-400 mr-3" />
                    <div>
                      <p className="text-sm font-medium text-gray-900">
                        Pedido #{pedido.id}
                      </p>
                      <p className="text-sm text-gray-500">
                        {pedido.usuario?.nombre} {pedido.usuario?.apellido}
                      </p>
                    </div>
                  </div>
                  <div className="text-right">
                    <p className="text-sm font-medium text-gray-900">
                      {formatCurrency(pedido.total)}
                    </p>
                    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                      pedido.estado === 'Completado' 
                        ? 'bg-green-100 text-green-800'
                        : pedido.estado === 'Pendiente'
                        ? 'bg-yellow-100 text-yellow-800'
                        : 'bg-gray-100 text-gray-800'
                    }`}>
                      {pedido.estado}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard; 