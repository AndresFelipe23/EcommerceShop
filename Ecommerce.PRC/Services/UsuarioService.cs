using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> ListarUsuariosAsync()
        {
            return await _usuarioRepository.ListarUsuariosAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _usuarioRepository.ObtenerPorIdAsync(id);
        }

        public async Task<int> InsertarUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepository.InsertarUsuarioAsync(usuario);
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepository.ActualizarUsuarioAsync(usuario);
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            return await _usuarioRepository.EliminarUsuarioAsync(id);
        }
    }
}
