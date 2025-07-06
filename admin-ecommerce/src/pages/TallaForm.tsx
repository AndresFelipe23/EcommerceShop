import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { ArrowLeft, Save, X } from 'lucide-react';
import { apiService } from '../services/api';


const TallaForm: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEditing = Boolean(id);

  console.log('TallaForm - ID recibido:', id, 'isEditing:', isEditing);

  const [formData, setFormData] = useState({
    talNombre: '',
    talGenero: '',
    talOrdenVisualizacion: null as number | null
  });

  const [errors, setErrors] = useState<Record<string, string>>({});
  const [isLoading, setIsLoading] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (isEditing) {
      fetchTalla();
    }
  }, [id]);

  const fetchTalla = async () => {
    if (!id) {
      console.error('ID no proporcionado');
      navigate('/tallas');
      return;
    }
    
    const tallaId = parseInt(id);
    if (isNaN(tallaId)) {
      console.error('ID inválido:', id);
      navigate('/tallas');
      return;
    }
    
    try {
      setIsLoading(true);
      const talla = await apiService.getTalla(tallaId);
      setFormData({
        talNombre: talla.talNombre,
        talGenero: talla.talGenero,
        talOrdenVisualizacion: talla.talOrdenVisualizacion
      });
    } catch (error) {
      console.error('Error al cargar talla:', error);
      navigate('/tallas');
    } finally {
      setIsLoading(false);
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};

    if (!formData.talNombre.trim()) {
      newErrors.talNombre = 'El nombre es requerido';
    } else if (formData.talNombre.length < 1) {
      newErrors.talNombre = 'El nombre debe tener al menos 1 carácter';
    } else if (formData.talNombre.length > 10) {
      newErrors.talNombre = 'El nombre no puede exceder 10 caracteres';
    }

    if (!formData.talGenero.trim()) {
      newErrors.talGenero = 'El género es requerido';
    } else if (formData.talGenero.length > 20) {
      newErrors.talGenero = 'El género no puede exceder 20 caracteres';
    }

    if (formData.talOrdenVisualizacion !== null && formData.talOrdenVisualizacion < 0) {
      newErrors.talOrdenVisualizacion = 'El orden de visualización debe ser mayor o igual a 0';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;

    try {
      setIsSubmitting(true);
      
      // Preparar los datos para enviar al backend
      const tallaData = {
        talNombre: formData.talNombre,
        talGenero: formData.talGenero,
        talOrdenVisualizacion: formData.talOrdenVisualizacion
      };
      
      if (isEditing) {
        if (!id) {
          console.error('ID no proporcionado para edición');
          return;
        }
        const tallaId = parseInt(id);
        if (isNaN(tallaId)) {
          console.error('ID inválido para edición:', id);
          return;
        }
        await apiService.updateTalla(tallaId, tallaData);
      } else {
        await apiService.createTalla(tallaData);
      }
      
      navigate('/tallas');
    } catch (error) {
      console.error('Error al guardar talla:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    
    setFormData(prev => ({
      ...prev,
      [name]: type === 'number' ? (value === '' ? null : parseInt(value)) : value
    }));

    // Limpiar error del campo cuando el usuario empiece a escribir
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
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
      <div className="flex items-center justify-between">
        <div className="flex items-center space-x-4">
          <Link
            to="/tallas"
            className="inline-flex items-center text-sm text-gray-500 hover:text-gray-700"
          >
            <ArrowLeft className="h-4 w-4 mr-1" />
            Volver
          </Link>
          <div>
            <h1 className="text-2xl font-bold text-gray-900">
              {isEditing ? 'Editar Talla' : 'Nueva Talla'}
            </h1>
            <p className="mt-1 text-sm text-gray-500">
              {isEditing ? 'Modifica los datos de la talla' : 'Crea una nueva talla'}
            </p>
          </div>
        </div>
      </div>

      {/* Formulario */}
      <div className="bg-white shadow rounded-lg">
        <form onSubmit={handleSubmit} className="space-y-6 p-6">
          {/* Nombre */}
          <div>
            <label htmlFor="talNombre" className="block text-sm font-medium text-gray-700">
              Nombre *
            </label>
            <input
              type="text"
              id="talNombre"
              name="talNombre"
              value={formData.talNombre}
              onChange={handleInputChange}
              className={`mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${
                errors.talNombre ? 'border-red-300' : ''
              }`}
              placeholder="Ej: S, M, L, XL"
            />
            {errors.talNombre && (
              <p className="mt-1 text-sm text-red-600">{errors.talNombre}</p>
            )}
          </div>

          {/* Género */}
          <div>
            <label htmlFor="talGenero" className="block text-sm font-medium text-gray-700">
              Género *
            </label>
            <input
              type="text"
              id="talGenero"
              name="talGenero"
              value={formData.talGenero}
              onChange={handleInputChange}
              className={`mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${
                errors.talGenero ? 'border-red-300' : ''
              }`}
              placeholder="Ej: Masculino, Femenino, Unisex"
            />
            {errors.talGenero && (
              <p className="mt-1 text-sm text-red-600">{errors.talGenero}</p>
            )}
          </div>

          {/* Orden de visualización */}
          <div>
            <label htmlFor="talOrdenVisualizacion" className="block text-sm font-medium text-gray-700">
              Orden de visualización
            </label>
            <input
              type="number"
              id="talOrdenVisualizacion"
              name="talOrdenVisualizacion"
              value={formData.talOrdenVisualizacion ?? ''}
              onChange={handleInputChange}
              className={`mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm ${
                errors.talOrdenVisualizacion ? 'border-red-300' : ''
              }`}
              placeholder="Orden de visualización"
            />
            {errors.talOrdenVisualizacion && (
              <p className="mt-1 text-sm text-red-600">{errors.talOrdenVisualizacion}</p>
            )}
          </div>

          {/* Botones */}
          <div className="flex justify-end space-x-3 pt-6 border-t border-gray-200">
            <Link
              to="/tallas"
              className="inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            >
              <X className="h-4 w-4 mr-2" />
              Cancelar
            </Link>
            <button
              type="submit"
              disabled={isSubmitting}
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <Save className="h-4 w-4 mr-2" />
              {isSubmitting ? 'Guardando...' : (isEditing ? 'Actualizar' : 'Crear')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default TallaForm; 