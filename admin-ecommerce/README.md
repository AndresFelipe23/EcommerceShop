# Panel de Administración - Ecommerce

Panel de administración moderno para el ecommerce desarrollado con React, TypeScript, Vite y Tailwind CSS.

## 🚀 Tecnologías Utilizadas

- **React 19** - Biblioteca de interfaz de usuario
- **TypeScript** - Tipado estático para JavaScript
- **Vite** - Herramienta de construcción rápida
- **Tailwind CSS** - Framework de CSS utilitario
- **React Router DOM** - Enrutamiento de la aplicación
- **Axios** - Cliente HTTP para las peticiones a la API
- **Lucide React** - Iconos modernos
- **Bun** - Runtime y gestor de paquetes

## 📁 Estructura del Proyecto

```
src/
├── components/          # Componentes reutilizables
│   └── Layout.tsx      # Layout principal con sidebar
├── contexts/           # Contextos de React
│   └── AuthContext.tsx # Contexto de autenticación
├── pages/              # Páginas de la aplicación
│   ├── Login.tsx       # Página de inicio de sesión
│   └── Dashboard.tsx   # Dashboard principal
├── services/           # Servicios de API
│   └── api.ts         # Cliente de API con Axios
├── types/              # Definiciones de tipos TypeScript
│   └── index.ts       # Tipos de la aplicación
├── hooks/              # Hooks personalizados
├── utils/              # Utilidades y helpers
├── App.tsx            # Componente principal
└── main.tsx           # Punto de entrada
```

## 🛠️ Instalación y Configuración

### Prerrequisitos

- **Bun** instalado en tu sistema
- **Node.js** (versión 18 o superior)

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone <url-del-repositorio>
   cd admin-ecommerce
   ```

2. **Instalar dependencias**
   ```bash
   bun install
   ```

3. **Configurar variables de entorno**
   ```bash
   # Crear archivo .env.local
   VITE_API_URL=https://localhost:7001/api
   ```

4. **Ejecutar en modo desarrollo**
   ```bash
   bun run dev
   ```

5. **Construir para producción**
   ```bash
   bun run build
   ```

## 🔧 Configuración de la API

El proyecto está configurado para conectarse a la API del ecommerce. Asegúrate de que:

1. La API esté ejecutándose en `https://localhost:7001`
2. Los endpoints coincidan con los definidos en `src/services/api.ts`
3. El CORS esté configurado correctamente en la API

## 📱 Características

### ✅ Implementadas

- **Autenticación completa** con JWT
- **Dashboard responsivo** con estadísticas
- **Layout moderno** con sidebar colapsible
- **Navegación protegida** por rutas
- **Gestión de estado** con Context API
- **Interfaz moderna** con Tailwind CSS
- **Iconos vectoriales** con Lucide React

### 🚧 En Desarrollo

- Gestión de productos (CRUD)
- Gestión de categorías
- Gestión de usuarios
- Gestión de pedidos
- Gestión de inventario
- Gestión de promociones y cupones
- Reportes y estadísticas avanzadas

## 🎨 Diseño y UX

- **Diseño responsivo** que funciona en móviles y desktop
- **Tema claro** con colores modernos
- **Animaciones suaves** y transiciones
- **Iconografía consistente** con Lucide React
- **Tipografía legible** con sistema de fuentes de Tailwind

## 🔐 Seguridad

- **Autenticación JWT** con interceptor automático
- **Protección de rutas** para usuarios no autenticados
- **Manejo de errores** de autenticación
- **Logout automático** en caso de token expirado

## 📊 Dashboard

El dashboard incluye:

- **Estadísticas principales**: Total de productos, usuarios, pedidos y ventas
- **Estadísticas secundarias**: Productos activos, usuarios activos, pedidos pendientes/completados
- **Productos recientes**: Lista de los últimos productos agregados
- **Pedidos recientes**: Lista de los últimos pedidos realizados

## 🚀 Scripts Disponibles

```bash
# Desarrollo
bun run dev          # Iniciar servidor de desarrollo
bun run build        # Construir para producción
bun run preview      # Vista previa de la construcción
bun run lint         # Ejecutar ESLint
bun run type-check   # Verificar tipos TypeScript
```

## 📝 Próximos Pasos

1. **Implementar CRUD completo** para todas las entidades
2. **Agregar filtros y búsqueda** en las listas
3. **Implementar paginación** para grandes volúmenes de datos
4. **Agregar reportes** y gráficos estadísticos
5. **Implementar notificaciones** en tiempo real
6. **Agregar modo oscuro** como opción
7. **Optimizar rendimiento** con lazy loading

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 📞 Soporte

Si tienes alguna pregunta o necesitas ayuda, no dudes en contactar al equipo de desarrollo.
