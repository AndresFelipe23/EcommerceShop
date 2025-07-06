using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface ICuponesUsuarioRepository
    {
        Task<IEnumerable<CuponesUsuario>> ListarCuponesUsuariosAsync();
        Task<CuponesUsuario?> ObtenerPorIdAsync(int id);
        Task<int> InsertarCuponesUsuarioAsync(CuponesUsuario cuponUsuario);
        Task<bool> ActualizarCuponesUsuarioAsync(CuponesUsuario cuponUsuario);
        Task<bool> EliminarCuponesUsuarioAsync(int id);
        Task<IEnumerable<CuponesUsuario>> ListarCuponesPorUsuarioAsync(int usuId);
        Task<IEnumerable<CuponesUsuario>> ListarCuponesUsadosPorUsuarioAsync(int usuId);
        Task<IEnumerable<CuponesUsuario>> ListarCuponesDisponiblesPorUsuarioAsync(int usuId);
        Task<CuponesUsuario?> ObtenerCuponUsuarioAsync(int cuponId, int usuId);
        Task<bool> MarcarCuponComoUsadoAsync(int cuponUsuarioId);
        Task<bool> AsignarCuponAUsuarioAsync(int cuponId, int usuId);
    }
} 