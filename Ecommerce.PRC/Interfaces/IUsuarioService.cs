using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ListarUsuariosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<int> InsertarUsuarioAsync(Usuario usuario);
        Task<bool> ActualizarUsuarioAsync(Usuario usuario);
        Task<bool> EliminarUsuarioAsync(int id);
    }
}