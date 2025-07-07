# Admin Ecommerce Frontend

Panel de administración para el sistema de ecommerce desarrollado en React con TypeScript.

## Configuración

### 1. Variables de Entorno

Crea un archivo `.env` en la raíz del proyecto con la siguiente configuración:

```env
VITE_API_URL=http://localhost:5268/api
```

### 2. Instalación de Dependencias

```bash
npm install
# o
bun install
```

### 3. Ejecutar el Backend

Antes de ejecutar el frontend, asegúrate de que el backend esté ejecutándose:

```bash
# Navegar al directorio del backend
cd ../Ecommerce.API

# Ejecutar el backend
dotnet run
```

El backend estará disponible en: http://localhost:5268

### 4. Ejecutar el Frontend

```bash
npm run dev
# o
bun dev
```

El frontend estará disponible en: http://localhost:3000

## Estructura del Proyecto

```
src/
├── components/     # Componentes reutilizables
├── contexts/       # Contextos de React (Auth, etc.)
├── pages/          # Páginas de la aplicación
├── services/       # Servicios de API
├── types/          # Definiciones de tipos TypeScript
├── config.ts       # Configuración de la aplicación
└── main.tsx        # Punto de entrada
```

## Funcionalidades

- **Autenticación**: Login/logout con JWT
- **Gestión de Usuarios**: CRUD de usuarios
- **Gestión de Productos**: CRUD de productos
- **Gestión de Categorías**: CRUD de categorías
- **Gestión de Tallas**: CRUD de tallas
- **Gestión de Colores**: CRUD de colores
- **Gestión de Inventario**: Control de stock
- **Gestión de Pedidos**: Seguimiento de pedidos
- **Dashboard**: Estadísticas y resumen

## Tecnologías Utilizadas

- **React 18** con TypeScript
- **Vite** como bundler
- **Tailwind CSS** para estilos
- **Axios** para peticiones HTTP
- **React Router** para navegación
- **React Hook Form** para formularios

## Desarrollo

### Scripts Disponibles

- `npm run dev` - Ejecutar en modo desarrollo
- `npm run build` - Construir para producción
- `npm run preview` - Previsualizar build de producción
- `npm run lint` - Ejecutar linter

### Convenciones de Código

- Usar TypeScript para todo el código
- Seguir las convenciones de React Hooks
- Usar componentes funcionales
- Implementar manejo de errores apropiado
- Documentar funciones complejas

## Solución de Problemas

### Error de Conexión al Backend

Si recibes errores de conexión:

1. Verifica que el backend esté ejecutándose en http://localhost:5268
2. Verifica la URL en el archivo `.env`
3. Revisa la consola del navegador para errores detallados

### Error de Autenticación

Si hay problemas con el login:

1. Verifica las credenciales en la base de datos
2. Revisa los logs del backend
3. Verifica que el JWT esté configurado correctamente

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request
