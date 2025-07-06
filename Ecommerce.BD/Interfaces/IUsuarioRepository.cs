using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ListarUsuariosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<int> InsertarUsuarioAsync(Usuario usuario);
        Task<bool> ActualizarUsuarioAsync(Usuario usuario);
        Task<bool> EliminarUsuarioAsync(int id);
    }
}