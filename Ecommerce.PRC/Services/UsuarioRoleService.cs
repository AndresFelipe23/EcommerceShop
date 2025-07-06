using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class UsuarioRoleService : IUsuarioRoleService
    {
        private readonly IUsuarioRoleRepository _usuarioRoleRepository;

        public UsuarioRoleService(IUsuarioRoleRepository usuarioRoleRepository)
        {
            _usuarioRoleRepository = usuarioRoleRepository;
        }

        public Task<IEnumerable<UsuarioRole>> ListarUsuarioRolesAsync()
            => _usuarioRoleRepository.ListarUsuarioRolesAsync();

        public Task<UsuarioRole?> ObtenerPorIdAsync(int id)
            => _usuarioRoleRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarUsuarioRoleAsync(UsuarioRole usuarioRol)
            => _usuarioRoleRepository.InsertarUsuarioRoleAsync(usuarioRol);

        public Task<bool> ActualizarUsuarioRoleAsync(UsuarioRole usuarioRol)
            => _usuarioRoleRepository.ActualizarUsuarioRoleAsync(usuarioRol);

        public Task<bool> EliminarUsuarioRoleAsync(int id)
            => _usuarioRoleRepository.EliminarUsuarioRoleAsync(id);
    }
}
