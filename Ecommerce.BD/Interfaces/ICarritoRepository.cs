using Ecommerce.BD.Models;

namespace Ecommerce.BD.Interfaces
{
    public interface ICarritoRepository
    {
        Task<IEnumerable<Carrito>> ListarAsync();
        Task<Carrito?> ObtenerPorIdAsync(int carritoId);
        Task<Carrito?> ObtenerPorUsuarioAsync(int usuId);
        Task<int> CrearAsync(Carrito carrito);
        Task<bool> ActualizarAsync(Carrito carrito);
        Task<bool> EliminarAsync(int carritoId);
        Task<bool> ExisteAsync(int carritoId);
        Task<bool> ExistePorUsuarioAsync(int usuId);
        Task<bool> VaciarCarritoAsync(int carritoId);
        Task<decimal> CalcularTotalAsync(int carritoId);
    }
} 