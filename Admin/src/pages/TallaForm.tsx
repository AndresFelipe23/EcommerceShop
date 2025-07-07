import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { ArrowLeft, Save, X, Tag, Users, Hash, AlertCircle, CheckCircle } from 'lucide-react';
import { apiService } from '../services/api';
import Swal from 'sweetalert2';

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
        talOrdenVisualizacion: talla.talOrdenVisualizacion ?? null
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
        
        // Mostrar mensaje de éxito
        Swal.fire({
          title: '¡Actualizado!',
          text: 'La talla ha sido actualizada exitosamente.',
          icon: 'success',
          timer: 2000,
          showConfirmButton: false
        });
      } else {
        await apiService.createTalla(tallaData);
        
        // Mostrar mensaje de éxito
        Swal.fire({
          title: '¡Creado!',
          text: 'La talla ha sido creada exitosamente.',
          icon: 'success',
          timer: 2000,
          showConfirmButton: false
        });
      }
      
      navigate('/tallas');
    } catch (error) {
      console.error('Error al guardar talla:', error);
      
      // Mostrar mensaje de error
      Swal.fire({
        title: 'Error',
        text: 'No se pudo guardar la talla. Por favor intenta de nuevo.',
        icon: 'error',
        confirmButtonText: 'Aceptar'
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
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
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-16 w-16 border-b-2 border-indigo-600 mx-auto mb-4"></div>
          <p className="text-gray-600 text-lg">Cargando talla...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen py-8">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Header mejorado */}
        <div className="mb-8">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <Link
                to="/tallas"
                className="inline-flex items-center px-4 py-2 text-sm font-medium text-indigo-600 bg-white rounded-lg border border-indigo-200 hover:bg-indigo-50 hover:border-indigo-300 transition-all duration-200 shadow-sm"
              >
                <ArrowLeft className="h-4 w-4 mr-2" />
                Volver a Tallas
              </Link>
            </div>
            <div className="text-right">
              <h1 className="text-3xl font-bold text-gray-900">
                {isEditing ? 'Editar Talla' : 'Nueva Talla'}
              </h1>
              <p className="mt-1 text-sm text-gray-600">
                {isEditing ? 'Modifica los datos de la talla existente' : 'Crea una nueva talla para tu catálogo'}
              </p>
            </div>
          </div>
        </div>

        {/* Formulario mejorado */}
        <div className="bg-white rounded-2xl shadow-xl border border-gray-100 overflow-hidden">
          {/* Header del formulario */}
          <div className="bg-gradient-to-r from-indigo-600 to-blue-600 px-8 py-6">
            <div className="flex items-center">
              <div className="flex-shrink-0">
                <div className="w-12 h-12 bg-white bg-opacity-20 rounded-lg flex items-center justify-center">
                  <Tag className="h-6 w-6 text-white" />
                </div>
              </div>
              <div className="ml-4">
                <h2 className="text-xl font-semibold text-white">
                  Información de la Talla
                </h2>
                <p className="text-indigo-100 text-sm">
                  Completa los datos básicos de la talla
                </p>
              </div>
            </div>
          </div>

          <form onSubmit={handleSubmit} className="p-8 space-y-8">
            {/* Grid de campos */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
              {/* Nombre */}
              <div className="space-y-2">
                <label htmlFor="talNombre" className="flex items-center text-sm font-semibold text-gray-700">
                  <Tag className="h-4 w-4 mr-2 text-indigo-600" />
                  Nombre de la Talla *
                </label>
                <div className="relative">
                  <input
                    type="text"
                    id="talNombre"
                    name="talNombre"
                    value={formData.talNombre}
                    onChange={handleInputChange}
                    className={`block w-full px-4 py-3 border-2 rounded-xl shadow-sm focus:ring-4 focus:ring-indigo-100 focus:border-indigo-500 transition-all duration-200 text-gray-900 placeholder-gray-400 ${
                      errors.talNombre 
                        ? 'border-red-300 bg-red-50' 
                        : 'border-gray-200 hover:border-gray-300'
                    }`}
                    placeholder="Ej: S, M, L, XL, 42, 44..."
                  />
                  {errors.talNombre ? (
                    <div className="absolute inset-y-0 right-0 flex items-center pr-3">
                      <AlertCircle className="h-5 w-5 text-red-500" />
                    </div>
                  ) : formData.talNombre && (
                    <div className="absolute inset-y-0 right-0 flex items-center pr-3">
                      <CheckCircle className="h-5 w-5 text-green-500" />
                    </div>
                  )}
                </div>
                {errors.talNombre && (
                  <p className="flex items-center text-sm text-red-600">
                    <AlertCircle className="h-4 w-4 mr-1" />
                    {errors.talNombre}
                  </p>
                )}
                <p className="text-xs text-gray-500">
                  Máximo 10 caracteres. Usa abreviaciones claras y reconocibles.
                </p>
              </div>

              {/* Género */}
              <div className="space-y-2">
                <label htmlFor="talGenero" className="flex items-center text-sm font-semibold text-gray-700">
                  <Users className="h-4 w-4 mr-2 text-indigo-600" />
                  Género *
                </label>
                <div className="relative">
                  <select
                    id="talGenero"
                    name="talGenero"
                    value={formData.talGenero}
                    onChange={handleInputChange}
                    className={`block w-full px-4 py-3 border-2 rounded-xl shadow-sm focus:ring-4 focus:ring-indigo-100 focus:border-indigo-500 transition-all duration-200 text-gray-900 appearance-none bg-white ${
                      errors.talGenero 
                        ? 'border-red-300 bg-red-50' 
                        : 'border-gray-200 hover:border-gray-300'
                    }`}
                  >
                    <option value="">Selecciona un género</option>
                    <option value="Masculino">Masculino</option>
                    <option value="Femenino">Femenino</option>
                    <option value="Unisex">Unisex</option>
                  </select>
                  <div className="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none">
                    <svg className="h-5 w-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                    </svg>
                  </div>
                  {errors.talGenero ? (
                    <div className="absolute inset-y-0 right-0 flex items-center pr-10">
                      <AlertCircle className="h-5 w-5 text-red-500" />
                    </div>
                  ) : formData.talGenero && (
                    <div className="absolute inset-y-0 right-0 flex items-center pr-10">
                      <CheckCircle className="h-5 w-5 text-green-500" />
                    </div>
                  )}
                </div>
                {errors.talGenero && (
                  <p className="flex items-center text-sm text-red-600">
                    <AlertCircle className="h-4 w-4 mr-1" />
                    {errors.talGenero}
                  </p>
                )}
                <p className="text-xs text-gray-500">
                  Define el público objetivo para esta talla.
                </p>
              </div>
            </div>

            {/* Orden de visualización - Campo completo */}
            <div className="space-y-2">
              <label htmlFor="talOrdenVisualizacion" className="flex items-center text-sm font-semibold text-gray-700">
                <Hash className="h-4 w-4 mr-2 text-indigo-600" />
                Orden de Visualización
              </label>
              <div className="relative max-w-xs">
                <input
                  type="number"
                  id="talOrdenVisualizacion"
                  name="talOrdenVisualizacion"
                  value={formData.talOrdenVisualizacion ?? ''}
                  onChange={handleInputChange}
                  className={`block w-full px-4 py-3 border-2 rounded-xl shadow-sm focus:ring-4 focus:ring-indigo-100 focus:border-indigo-500 transition-all duration-200 text-gray-900 placeholder-gray-400 ${
                    errors.talOrdenVisualizacion 
                      ? 'border-red-300 bg-red-50' 
                      : 'border-gray-200 hover:border-gray-300'
                  }`}
                  placeholder="Ej: 1, 2, 3..."
                  min="0"
                />
                {errors.talOrdenVisualizacion && (
                  <div className="absolute inset-y-0 right-0 flex items-center pr-3">
                    <AlertCircle className="h-5 w-5 text-red-500" />
                  </div>
                )}
              </div>
              {errors.talOrdenVisualizacion && (
                <p className="flex items-center text-sm text-red-600">
                  <AlertCircle className="h-4 w-4 mr-1" />
                  {errors.talOrdenVisualizacion}
                </p>
              )}
              <p className="text-xs text-gray-500">
                Define el orden en que aparecerá esta talla en los listados. Deja vacío para usar el orden por defecto.
              </p>
            </div>

            {/* Separador visual */}
            <div className="border-t border-gray-200 my-8"></div>

            {/* Botones mejorados */}
            <div className="flex flex-col sm:flex-row justify-end space-y-3 sm:space-y-0 sm:space-x-4">
              <Link
                to="/tallas"
                className="inline-flex items-center justify-center px-6 py-3 border-2 border-gray-300 shadow-sm text-base font-medium rounded-xl text-gray-700 bg-white hover:bg-gray-50 hover:border-gray-400 focus:outline-none focus:ring-4 focus:ring-gray-100 transition-all duration-200"
              >
                <X className="h-5 w-5 mr-2" />
                Cancelar
              </Link>
              <button
                type="submit"
                disabled={isSubmitting}
                className="inline-flex items-center justify-center px-8 py-3 border border-transparent text-base font-medium rounded-xl shadow-sm text-white bg-gradient-to-r from-indigo-600 to-blue-600 hover:from-indigo-700 hover:to-blue-700 focus:outline-none focus:ring-4 focus:ring-indigo-100 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200 transform hover:scale-105"
              >
                <Save className="h-5 w-5 mr-2" />
                {isSubmitting ? (
                  <>
                    <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                    Guardando...
                  </>
                ) : (
                  isEditing ? 'Actualizar Talla' : 'Crear Talla'
                )}
              </button>
            </div>
          </form>
        </div>

        {/* Información adicional */}
        <div className="mt-8 bg-white rounded-xl shadow-lg border border-gray-100 p-6">
          <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center">
            <AlertCircle className="h-5 w-5 mr-2 text-blue-600" />
            Información Importante
          </h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-sm text-gray-600">
            <div>
              <p className="font-medium text-gray-700 mb-2">Consejos para el nombre:</p>
              <ul className="space-y-1">
                <li>• Usa abreviaciones estándar (S, M, L, XL)</li>
                <li>• Para calzado, usa números (42, 44, 46)</li>
                <li>• Mantén consistencia en tu catálogo</li>
              </ul>
            </div>
            <div>
              <p className="font-medium text-gray-700 mb-2">Orden de visualización:</p>
              <ul className="space-y-1">
                <li>• Números menores aparecen primero</li>
                <li>• Útil para organizar tallas lógicamente</li>
                <li>• Puedes dejarlo vacío para orden alfabético</li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TallaForm; 