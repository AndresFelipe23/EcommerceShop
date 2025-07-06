using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class DireccionService : IDireccionService
    {
        private readonly IDireccionRepository _direccionRepository;

        public DireccionService(IDireccionRepository direccionRepository)
        {
            _direccionRepository = direccionRepository;
        }

        public async Task<IEnumerable<Direccione>> ListarAsync()
        {
            return await _direccionRepository.ListarAsync();
        }

        public async Task<Direccione?> ObtenerPorIdAsync(int dirId)
        {
            if (dirId <= 0)
                throw new ArgumentException("El ID de dirección debe ser mayor a 0", nameof(dirId));

            return await _direccionRepository.ObtenerPorIdAsync(dirId);
        }

        public async Task<IEnumerable<Direccione>> ListarPorUsuarioAsync(int usuId)
        {
            if (usuId <= 0)
                throw new ArgumentException("El ID de usuario debe ser mayor a 0", nameof(usuId));

            return await _direccionRepository.ListarPorUsuarioAsync(usuId);
        }

        public async Task<int> CrearAsync(Direccione direccion)
        {
            if (direccion == null)
                throw new ArgumentNullException(nameof(direccion));

            if (string.IsNullOrWhiteSpace(direccion.DirNombre))
                throw new ArgumentException("El nombre de la dirección es requerido", nameof(direccion.DirNombre));

            if (string.IsNullOrWhiteSpace(direccion.DirPais))
                throw new ArgumentException("El país es requerido", nameof(direccion.DirPais));

            if (string.IsNullOrWhiteSpace(direccion.DirCiudad))
                throw new ArgumentException("La ciudad es requerida", nameof(direccion.DirCiudad));

            if (string.IsNullOrWhiteSpace(direccion.DirLinea1))
                throw new ArgumentException("La dirección es requerida", nameof(direccion.DirLinea1));

            if (direccion.UsuId <= 0)
                throw new ArgumentException("El ID de usuario es requerido", nameof(direccion.UsuId));

            return await _direccionRepository.CrearAsync(direccion);
        }

        public async Task<bool> ActualizarAsync(Direccione direccion)
        {
            if (direccion == null)
                throw new ArgumentNullException(nameof(direccion));

            if (direccion.DirId <= 0)
                throw new ArgumentException("El ID de dirección debe ser mayor a 0", nameof(direccion.DirId));

            if (string.IsNullOrWhiteSpace(direccion.DirNombre))
                throw new ArgumentException("El nombre de la dirección es requerido", nameof(direccion.DirNombre));

            if (string.IsNullOrWhiteSpace(direccion.DirPais))
                throw new ArgumentException("El país es requerido", nameof(direccion.DirPais));

            if (string.IsNullOrWhiteSpace(direccion.DirCiudad))
                throw new ArgumentException("La ciudad es requerida", nameof(direccion.DirCiudad));

            if (string.IsNullOrWhiteSpace(direccion.DirLinea1))
                throw new ArgumentException("La dirección es requerida", nameof(direccion.DirLinea1));

            // Verificar que la dirección existe
            var existe = await _direccionRepository.ExisteAsync(direccion.DirId);
            if (!existe)
                throw new InvalidOperationException($"La dirección con ID {direccion.DirId} no existe");

            return await _direccionRepository.ActualizarAsync(direccion);
        }

        public async Task<bool> EliminarAsync(int dirId)
        {
            if (dirId <= 0)
                throw new ArgumentException("El ID de dirección debe ser mayor a 0", nameof(dirId));

            // Verificar que la dirección existe
            var existe = await _direccionRepository.ExisteAsync(dirId);
            if (!existe)
                throw new InvalidOperationException($"La dirección con ID {dirId} no existe");

            return await _direccionRepository.EliminarAsync(dirId);
        }

        public async Task<bool> MarcarComoPrincipalAsync(int dirId, int usuId)
        {
            if (dirId <= 0)
                throw new ArgumentException("El ID de dirección debe ser mayor a 0", nameof(dirId));

            if (usuId <= 0)
                throw new ArgumentException("El ID de usuario debe ser mayor a 0", nameof(usuId));

            // Verificar que la dirección existe
            var existe = await _direccionRepository.ExisteAsync(dirId);
            if (!existe)
                throw new InvalidOperationException($"La dirección con ID {dirId} no existe");

            return await _direccionRepository.MarcarComoPrincipalAsync(dirId, usuId);
        }

        public async Task<Direccione?> ObtenerDireccionPrincipalAsync(int usuId)
        {
            if (usuId <= 0)
                throw new ArgumentException("El ID de usuario debe ser mayor a 0", nameof(usuId));

            var direcciones = await _direccionRepository.ListarPorUsuarioAsync(usuId);
            return direcciones.FirstOrDefault(d => d.DirEsPrincipal == true);
        }
    }
} 