import React, { useState, useEffect } from 'react';
import { Plus, Search, Edit, Trash2, Filter } from 'lucide-react';
import { Link } from 'react-router-dom';
import { apiService } from '../services/api';
import type { Talla } from '@/types';


const Tallas: React.FC = () => {
  const [tallas, setTallas] = useState<Talla[]>([]);
  const [filteredTallas, setFilteredTallas] = useState<Talla[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedTalla, setSelectedTalla] = useState<Talla | null>(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);

  useEffect(() => {
    fetchTallas();
  }, []);

  useEffect(() => {
    filterTallas();
  }, [tallas, searchTerm]);

  const fetchTallas = async () => {
    try {
      setIsLoading(true);
      const data = await apiService.getTallas();
      console.log('Tallas obtenidas:', data);
      setTallas(data);
    } catch (error) {
      console.error('Error al cargar tallas:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const filterTallas = () => {
    let filtered = tallas;
    
    if (searchTerm) {
      filtered = filtered.filter(talla =>
        talla.talNombre.toLowerCase().includes(searchTerm.toLowerCase()) ||
        talla.talGenero.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }
    
    setFilteredTallas(filtered);
  };

  const handleDelete = async () => {
    if (!selectedTalla) return;
    
    try {
      await apiService.deleteTalla(selectedTalla.TallaId);
      setTallas(tallas.filter(t => t.TallaId !== selectedTalla.TallaId));
      setShowDeleteModal(false);
      setSelectedTalla(null);
    } catch (error) {
      console.error('Error al eliminar talla:', error);
    }
  };

  const confirmDelete = (talla: Talla) => {
    setSelectedTalla(talla);
    setShowDeleteModal(true);
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Tallas</h1>
          <p className="mt-1 text-sm text-gray-500">
            Gestiona las tallas de los productos
          </p>
        </div>
        <Link
          to="/tallas/nueva"
          className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          <Plus className="h-4 w-4 mr-2" />
          Nueva Talla
        </Link>
      </div>

      {/* Filtros y búsqueda */}
      <div className="bg-white shadow rounded-lg p-6">
        <div className="flex flex-col sm:flex-row gap-4">
          <div className="flex-1">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <input
                type="text"
                placeholder="Buscar tallas..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
              />
            </div>
          </div>
          <div className="flex gap-2">
            <button className="inline-flex items-center px-3 py-2 border border-gray-300 shadow-sm text-sm leading-4 font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
              <Filter className="h-4 w-4 mr-2" />
              Filtros
            </button>
          </div>
        </div>
      </div>

      {/* Tabla */}
      <div className="bg-white shadow overflow-hidden sm:rounded-md">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  ID
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Nombre
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Género
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Orden
                </th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Acciones
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {filteredTallas.map((talla) => (
                <tr key={talla.TallaId} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {talla.TallaId}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {talla.talNombre}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {talla.talGenero}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {talla.talOrdenVisualizacion || '-'}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <div className="flex justify-end space-x-2">
                      <Link
                        to={`/tallas/editar/${talla.TallaId}`}
                        className="text-indigo-600 hover:text-indigo-900"
                        onClick={() => {
                          console.log('Editando talla completa:', talla);
                          console.log('Editando talla con ID:', talla.TallaId, 'URL:', `/tallas/editar/${talla.TallaId}`);
                        }}
                      >
                        <Edit className="h-4 w-4" />
                      </Link>
                      <button
                        onClick={() => confirmDelete(talla)}
                        className="text-red-600 hover:text-red-900"
                      >
                        <Trash2 className="h-4 w-4" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        
        {filteredTallas.length === 0 && (
          <div className="text-center py-12">
            <p className="text-gray-500">No se encontraron tallas</p>
          </div>
        )}
      </div>

      {/* Modal de confirmación de eliminación */}
      {showDeleteModal && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
            <div className="mt-3 text-center">
              <h3 className="text-lg font-medium text-gray-900 mb-4">
                Confirmar eliminación
              </h3>
              <p className="text-sm text-gray-500 mb-6">
                ¿Estás seguro de que quieres eliminar la talla "{selectedTalla?.talNombre}"? 
                Esta acción no se puede deshacer.
              </p>
              <div className="flex justify-center space-x-4">
                <button
                  onClick={() => setShowDeleteModal(false)}
                  className="px-4 py-2 bg-gray-300 text-gray-700 rounded-md hover:bg-gray-400 focus:outline-none focus:ring-2 focus:ring-gray-500"
                >
                  Cancelar
                </button>
                <button
                  onClick={handleDelete}
                  className="px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500"
                >
                  Eliminar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Tallas; 