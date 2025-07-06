using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IUsuarioRoleRepository
    {
        Task<IEnumerable<UsuarioRole>> ListarUsuarioRolesAsync();
        Task<UsuarioRole?> ObtenerPorIdAsync(int id);
        Task<int> InsertarUsuarioRoleAsync(UsuarioRole usuarioRol);
        Task<bool> ActualizarUsuarioRoleAsync(UsuarioRole usuarioRol);
        Task<bool> EliminarUsuarioRoleAsync(int id);
    }
}