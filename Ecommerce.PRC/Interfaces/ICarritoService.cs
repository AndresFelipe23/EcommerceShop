using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface ICarritoService
    {
        Task<IEnumerable<Carrito>> ListarAsync();
        Task<Carrito?> ObtenerPorIdAsync(int carritoId);
        Task<Carrito?> ObtenerPorUsuarioAsync(int usuId);
        Task<int> CrearAsync(Carrito carrito);
        Task<bool> ActualizarAsync(Carrito carrito);
        Task<bool> EliminarAsync(int carritoId);
        Task<bool> VaciarCarritoAsync(int carritoId);
        Task<decimal> CalcularTotalAsync(int carritoId);
        Task<Carrito> ObtenerOCrearCarritoAsync(int usuId);
        Task<object> ObtenerResumenCarritoAsync(int carritoId);
    }
} 