using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class CuponesUsuarioService : ICuponesUsuarioService
    {
        private readonly ICuponesUsuarioRepository _cuponesUsuarioRepository;

        public CuponesUsuarioService(ICuponesUsuarioRepository cuponesUsuarioRepository)
        {
            _cuponesUsuarioRepository = cuponesUsuarioRepository;
        }

        public Task<IEnumerable<CuponesUsuario>> ListarCuponesUsuariosAsync()
            => _cuponesUsuarioRepository.ListarCuponesUsuariosAsync();

        public Task<CuponesUsuario?> ObtenerPorIdAsync(int id)
            => _cuponesUsuarioRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarCuponesUsuarioAsync(CuponesUsuario cuponUsuario)
            => _cuponesUsuarioRepository.InsertarCuponesUsuarioAsync(cuponUsuario);

        public Task<bool> ActualizarCuponesUsuarioAsync(CuponesUsuario cuponUsuario)
            => _cuponesUsuarioRepository.ActualizarCuponesUsuarioAsync(cuponUsuario);

        public Task<bool> EliminarCuponesUsuarioAsync(int id)
            => _cuponesUsuarioRepository.EliminarCuponesUsuarioAsync(id);

        public Task<IEnumerable<CuponesUsuario>> ListarCuponesPorUsuarioAsync(int usuId)
            => _cuponesUsuarioRepository.ListarCuponesPorUsuarioAsync(usuId);

        public Task<IEnumerable<CuponesUsuario>> ListarCuponesUsadosPorUsuarioAsync(int usuId)
            => _cuponesUsuarioRepository.ListarCuponesUsadosPorUsuarioAsync(usuId);

        public Task<IEnumerable<CuponesUsuario>> ListarCuponesDisponiblesPorUsuarioAsync(int usuId)
            => _cuponesUsuarioRepository.ListarCuponesDisponiblesPorUsuarioAsync(usuId);

        public Task<CuponesUsuario?> ObtenerCuponUsuarioAsync(int cuponId, int usuId)
            => _cuponesUsuarioRepository.ObtenerCuponUsuarioAsync(cuponId, usuId);

        public Task<bool> MarcarCuponComoUsadoAsync(int cuponUsuarioId)
            => _cuponesUsuarioRepository.MarcarCuponComoUsadoAsync(cuponUsuarioId);

        public Task<bool> AsignarCuponAUsuarioAsync(int cuponId, int usuId)
            => _cuponesUsuarioRepository.AsignarCuponAUsuarioAsync(cuponId, usuId);
    }
} 