using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public Task<IEnumerable<Role>> ListarRolesAsync()
            => _rolRepository.ListarRolesAsync();

        public Task<Role?> ObtenerPorIdAsync(int id)
            => _rolRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarRolAsync(Role rol)
            => _rolRepository.InsertarRolAsync(rol);

        public Task<bool> ActualizarRolAsync(Role rol)
            => _rolRepository.ActualizarRolAsync(rol);

        public Task<bool> EliminarRolAsync(int id)
            => _rolRepository.EliminarRolAsync(id);
    }
}
