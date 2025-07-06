using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<Role>> ListarRolesAsync();
        Task<Role?> ObtenerPorIdAsync(int id);
        Task<int> InsertarRolAsync(Role rol);
        Task<bool> ActualizarRolAsync(Role rol);
        Task<bool> EliminarRolAsync(int id);
    }
}