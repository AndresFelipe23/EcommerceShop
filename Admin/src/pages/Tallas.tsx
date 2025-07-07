import React, { useState, useEffect } from 'react';
import { Plus, Search, Edit, Trash2, Filter, Tag, Users, Hash } from 'lucide-react';
import { Link } from 'react-router-dom';
import { apiService } from '../services/api';
import type { Talla } from '@/types';
import Swal from 'sweetalert2';

const Tallas: React.FC = () => {
  const [tallas, setTallas] = useState<Talla[]>([]);
  const [filteredTallas, setFilteredTallas] = useState<Talla[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedGenero, setSelectedGenero] = useState<string>('');
  const [sortBy, setSortBy] = useState<'nombre' | 'genero' | 'orden'>('nombre');
  const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('asc');

  useEffect(() => {
    fetchTallas();
  }, []);

  useEffect(() => {
    filterTallas();
  }, [tallas, searchTerm, selectedGenero, sortBy, sortOrder]);

  const fetchTallas = async () => {
    try {
      setIsLoading(true);
      const data = await apiService.getTallas();
      console.log('Tallas obtenidas:', data);
      console.log('Tipo de data:', typeof data);
      console.log('Es array:', Array.isArray(data));
      console.log('Longitud:', data.length);
      if (data.length > 0) {
        console.log('Primera talla:', data[0]);
        console.log('Propiedades de la primera talla:', Object.keys(data[0]));
        console.log('tallaId de la primera talla:', data[0].tallaId);
      }
      setTallas(data);
    } catch (error) {
      console.error('Error al cargar tallas:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const filterTallas = () => {
    let filtered = tallas;
    
    // Filtro por búsqueda de texto
    if (searchTerm) {
      filtered = filtered.filter(talla =>
        (talla.talNombre || '').toLowerCase().includes(searchTerm.toLowerCase()) ||
        (talla.talGenero || '').toLowerCase().includes(searchTerm.toLowerCase()) ||
        (talla.tallaId?.toString() || '').includes(searchTerm)
      );
    }
    
    // Filtro por género
    if (selectedGenero) {
      filtered = filtered.filter(talla =>
        talla.talGenero?.toLowerCase() === selectedGenero.toLowerCase()
      );
    }
    
    // Ordenamiento
    filtered.sort((a, b) => {
      let aValue: string | number;
      let bValue: string | number;
      
      switch (sortBy) {
        case 'nombre':
          aValue = a.talNombre || '';
          bValue = b.talNombre || '';
          break;
        case 'genero':
          aValue = a.talGenero || '';
          bValue = b.talGenero || '';
          break;
        case 'orden':
          aValue = a.talOrdenVisualizacion ?? 0;
          bValue = b.talOrdenVisualizacion ?? 0;
          break;
        default:
          aValue = a.talNombre || '';
          bValue = b.talNombre || '';
      }
      
      if (sortOrder === 'asc') {
        return aValue > bValue ? 1 : -1;
      } else {
        return aValue < bValue ? 1 : -1;
      }
    });
    
    setFilteredTallas(filtered);
  };

  const confirmDelete = async (talla: Talla) => {
    const tallaId = talla.tallaId;
    const tallaNombre = talla.talNombre || 'esta talla';
    
    const result = await Swal.fire({
      title: '¿Estás seguro?',
      text: `¿Estás seguro de que quieres eliminar la talla "${tallaNombre}"? Esta acción no se puede deshacer.`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
      reverseButtons: true
    });

    if (result.isConfirmed) {
      try {
        // Mostrar loading
        Swal.fire({
          title: 'Eliminando...',
          text: 'Por favor espera mientras se elimina la talla',
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          }
        });

        await apiService.deleteTalla(tallaId);
        
        // Actualizar la lista local
        setTallas(tallas.filter(t => t.tallaId !== tallaId));
        
        // Mostrar mensaje de éxito
        Swal.fire({
          title: '¡Eliminado!',
          text: `La talla "${tallaNombre}" ha sido eliminada exitosamente.`,
          icon: 'success',
          timer: 2000,
          showConfirmButton: false
        });
        
      } catch (error) {
        console.error('Error al eliminar talla:', error);
        
        // Mostrar mensaje de error
        Swal.fire({
          title: 'Error',
          text: 'No se pudo eliminar la talla. Por favor intenta de nuevo.',
          icon: 'error',
          confirmButtonText: 'Aceptar'
        });
      }
    }
  };

  const getGeneroColor = (genero: string) => {
    switch (genero?.toLowerCase()) {
      case 'masculino':
        return 'bg-blue-100 text-blue-800 border-blue-200';
      case 'femenino':
        return 'bg-pink-100 text-pink-800 border-pink-200';
      case 'unisex':
        return 'bg-purple-100 text-purple-800 border-purple-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  };

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-16 w-16 border-b-2 border-indigo-600 mx-auto mb-4"></div>
          <p className="text-gray-600 text-lg">Cargando tallas...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Header mejorado */}
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center space-y-4 sm:space-y-0">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 flex items-center">
            <Tag className="h-8 w-8 mr-3 text-indigo-600" />
            Gestión de Tallas
          </h1>
          <p className="mt-2 text-gray-600">
            Administra las tallas disponibles para tus productos
          </p>
        </div>
        <Link
          to="/tallas/nueva"
          className="inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-xl shadow-sm text-white bg-gradient-to-r from-indigo-600 to-blue-600 hover:from-indigo-700 hover:to-blue-700 focus:outline-none focus:ring-4 focus:ring-indigo-100 transition-all duration-200 transform hover:scale-105"
        >
          <Plus className="h-5 w-5 mr-2" />
          Nueva Talla
        </Link>
      </div>

      {/* Estadísticas rápidas */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <div className="bg-white rounded-xl shadow-lg border border-gray-100 p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <div className="w-12 h-12 bg-indigo-100 rounded-lg flex items-center justify-center">
                <Tag className="h-6 w-6 text-indigo-600" />
              </div>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Total Tallas</p>
              <p className="text-2xl font-bold text-gray-900">{tallas.length}</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-lg border border-gray-100 p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                <Users className="h-6 w-6 text-blue-600" />
              </div>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Masculino</p>
              <p className="text-2xl font-bold text-gray-900">
                {tallas.filter(t => t.talGenero?.toLowerCase() === 'masculino').length}
              </p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-lg border border-gray-100 p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <div className="w-12 h-12 bg-pink-100 rounded-lg flex items-center justify-center">
                <Users className="h-6 w-6 text-pink-600" />
              </div>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Femenino</p>
              <p className="text-2xl font-bold text-gray-900">
                {tallas.filter(t => t.talGenero?.toLowerCase() === 'femenino').length}
              </p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-lg border border-gray-100 p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center">
                <Users className="h-6 w-6 text-purple-600" />
              </div>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-500">Unisex</p>
              <p className="text-2xl font-bold text-gray-900">
                {tallas.filter(t => t.talGenero?.toLowerCase() === 'unisex').length}
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* Filtros y búsqueda mejorados */}
      <div className="bg-white rounded-xl shadow-lg border border-gray-100 p-6">
        <div className="space-y-4">
          {/* Búsqueda */}
          <div className="relative">
            <Search className="absolute left-4 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-400" />
            <input
              type="text"
              placeholder="Buscar por nombre, género o ID..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="block w-full pl-12 pr-4 py-3 border-2 border-gray-200 rounded-xl leading-5 bg-white placeholder-gray-500 focus:outline-none focus:ring-4 focus:ring-indigo-100 focus:border-indigo-500 transition-all duration-200 text-gray-900"
            />
          </div>
          
          {/* Filtros y ordenamiento */}
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            {/* Filtro por género */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Filtrar por género</label>
              <select
                value={selectedGenero}
                onChange={(e) => setSelectedGenero(e.target.value)}
                className="block w-full px-3 py-2 border-2 border-gray-200 rounded-lg focus:ring-4 focus:ring-indigo-100 focus:border-indigo-500 transition-all duration-200"
              >
                <option value="">Todos los géneros</option>
                <option value="Masculino">Masculino</option>
                <option value="Femenino">Femenino</option>
                <option value="Unisex">Unisex</option>
              </select>
            </div>
            
            {/* Ordenar por */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Ordenar por</label>
              <select
                value={sortBy}
                onChange={(e) => setSortBy(e.target.value as 'nombre' | 'genero' | 'orden')}
                className="block w-full px-3 py-2 border-2 border-gray-200 rounded-lg focus:ring-4 focus:ring-indigo-100 focus:border-indigo-500 transition-all duration-200"
              >
                <option value="nombre">Nombre</option>
                <option value="genero">Género</option>
                <option value="orden">Orden</option>
              </select>
            </div>
            
            {/* Orden ascendente/descendente */}
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">Orden</label>
              <select
                value={sortOrder}
                onChange={(e) => setSortOrder(e.target.value as 'asc' | 'desc')}
                className="block w-full px-3 py-2 border-2 border-gray-200 rounded-lg focus:ring-4 focus:ring-indigo-100 focus:border-indigo-500 transition-all duration-200"
              >
                <option value="asc">Ascendente</option>
                <option value="desc">Descendente</option>
              </select>
            </div>
            
            {/* Botón limpiar */}
            <div className="flex items-end">
              <button 
                onClick={() => {
                  setSearchTerm('');
                  setSelectedGenero('');
                  setSortBy('nombre');
                  setSortOrder('asc');
                }}
                className="w-full inline-flex items-center justify-center px-4 py-2 border-2 border-gray-200 shadow-sm text-sm font-medium rounded-lg text-gray-700 bg-white hover:bg-gray-50 hover:border-gray-300 focus:outline-none focus:ring-4 focus:ring-indigo-100 transition-all duration-200"
              >
                <Filter className="h-4 w-4 mr-2" />
                Limpiar Filtros
              </button>
            </div>
          </div>
          
          {/* Resultados */}
          <div className="text-sm text-gray-600">
            Mostrando {filteredTallas.length} de {tallas.length} tallas
            {searchTerm && ` que coinciden con "${searchTerm}"`}
            {selectedGenero && ` del género "${selectedGenero}"`}
          </div>
        </div>
      </div>

      {/* Tabla mejorada */}
      <div className="bg-white rounded-xl shadow-lg border border-gray-100 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gradient-to-r from-indigo-50 to-blue-50">
              <tr>
                <th className="px-6 py-4 text-left text-xs font-semibold text-gray-700 uppercase tracking-wider">
                  <div className="flex items-center">
                    <Hash className="h-4 w-4 mr-2" />
                    ID
                  </div>
                </th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-gray-700 uppercase tracking-wider">
                  <div className="flex items-center">
                    <Tag className="h-4 w-4 mr-2" />
                    Nombre
                  </div>
                </th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-gray-700 uppercase tracking-wider">
                  <div className="flex items-center">
                    <Users className="h-4 w-4 mr-2" />
                    Género
                  </div>
                </th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-gray-700 uppercase tracking-wider">
                  <div className="flex items-center">
                    <Hash className="h-4 w-4 mr-2" />
                    Orden
                  </div>
                </th>
                <th className="px-6 py-4 text-right text-xs font-semibold text-gray-700 uppercase tracking-wider">
                  Acciones
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-100">
              {filteredTallas.map((talla, index) => {
                const tallaId = talla?.tallaId || `talla-${index}`;
                console.log('Renderizando talla:', talla, 'ID:', tallaId, 'Index:', index);
                return (
                  <tr key={tallaId} className="hover:bg-gray-50 transition-colors duration-200">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <div className="flex-shrink-0 h-8 w-8">
                          <div className="h-8 w-8 rounded-lg bg-indigo-100 flex items-center justify-center">
                            <span className="text-sm font-semibold text-indigo-600">
                              {talla?.tallaId || 'N/A'}
                            </span>
                          </div>
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-semibold text-gray-900">
                        {talla?.talNombre || 'N/A'}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-medium border ${getGeneroColor(talla?.talGenero || '')}`}>
                        {talla?.talGenero || 'N/A'}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {talla?.talOrdenVisualizacion || '-'}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <div className="flex justify-end space-x-2">
                        <Link
                          to={`/tallas/editar/${tallaId}`}
                          className="inline-flex items-center px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-lg text-indigo-700 bg-indigo-100 hover:bg-indigo-200 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 transition-all duration-200"
                          onClick={() => {
                            console.log('Editando talla completa:', talla);
                            console.log('Editando talla con ID:', tallaId, 'URL:', `/tallas/editar/${tallaId}`);
                          }}
                        >
                          <Edit className="h-4 w-4 mr-1" />
                          Editar
                        </Link>
                        <button
                          onClick={() => confirmDelete(talla)}
                          className="inline-flex items-center px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-lg text-red-700 bg-red-100 hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 transition-all duration-200"
                        >
                          <Trash2 className="h-4 w-4 mr-1" />
                          Eliminar
                        </button>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
        
        {filteredTallas.length === 0 && (
          <div className="text-center py-16">
            <div className="mx-auto h-24 w-24 text-gray-300 mb-4">
              <Tag className="h-24 w-24" />
            </div>
            <h3 className="text-lg font-medium text-gray-900 mb-2">
              {searchTerm ? 'No se encontraron tallas' : 'No hay tallas registradas'}
            </h3>
            <p className="text-gray-500 mb-6">
              {searchTerm 
                ? 'Intenta con otros términos de búsqueda' 
                : 'Comienza creando tu primera talla'
              }
            </p>
            {!searchTerm && (
              <Link
                to="/tallas/nueva"
                className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-lg shadow-sm text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
              >
                <Plus className="h-4 w-4 mr-2" />
                Crear Primera Talla
              </Link>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default Tallas; 