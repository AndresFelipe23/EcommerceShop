using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class CarritoItemRepository : ICarritoItemRepository
    {
        private readonly DapperContext _context;

        public CarritoItemRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarritoItem>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CarritoItem>("CarritoItem_List");
        }

        public async Task<CarritoItem?> ObtenerPorIdAsync(int carritoItemId)
        {
            using var connection = _context.CreateConnection();
            var items = await connection.QueryAsync<CarritoItem>(
                "SELECT * FROM CarritoItem WHERE CarritoItemId = @CarritoItemId",
                new { CarritoItemId = carritoItemId }
            );
            return items.FirstOrDefault();
        }

        public async Task<IEnumerable<CarritoItem>> ListarPorCarritoAsync(int carritoId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CarritoItem>(
                "CarritoItem_ListPorCarrito",
                new { CarritoId = carritoId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CrearAsync(CarritoItem carritoItem)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@CarritoId", carritoItem.CarritoId);
            parameters.Add("@ProductoTallaColorId", carritoItem.ProductoTallaColorId);
            parameters.Add("@Cantidad", carritoItem.Cantidad);
            parameters.Add("@PrecioUnitario", carritoItem.PrecioUnitario);
            parameters.Add("@CarritoItemId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "CarritoItem_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@CarritoItemId");
        }

        public async Task<bool> ActualizarAsync(CarritoItem carritoItem)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@CarritoItemId", carritoItem.CarritoItemId);
            parameters.Add("@Cantidad", carritoItem.Cantidad);
            parameters.Add("@PrecioUnitario", carritoItem.PrecioUnitario);

            var rowsAffected = await connection.ExecuteAsync(
                "CarritoItem_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int carritoItemId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "CarritoItem_Delete",
                new { CarritoItemId = carritoItemId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int carritoItemId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM CarritoItem WHERE CarritoItemId = @CarritoItemId",
                new { CarritoItemId = carritoItemId }
            );
            return count > 0;
        }

        public async Task<bool> ExisteProductoEnCarritoAsync(int carritoId, int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM CarritoItem WHERE CarritoId = @CarritoId AND ProductoTallaColorId = @ProductoTallaColorId",
                new { CarritoId = carritoId, ProductoTallaColorId = productoTallaColorId }
            );
            return count > 0;
        }

        public async Task<CarritoItem?> ObtenerPorProductoAsync(int carritoId, int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            var items = await connection.QueryAsync<CarritoItem>(
                "SELECT * FROM CarritoItem WHERE CarritoId = @CarritoId AND ProductoTallaColorId = @ProductoTallaColorId",
                new { CarritoId = carritoId, ProductoTallaColorId = productoTallaColorId }
            );
            return items.FirstOrDefault();
        }

        public async Task<bool> ActualizarCantidadAsync(int carritoItemId, int cantidad)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE CarritoItem SET Cantidad = @Cantidad WHERE CarritoItemId = @CarritoItemId",
                new { CarritoItemId = carritoItemId, Cantidad = cantidad }
            );
            return rowsAffected > 0;
        }

        public async Task<int> ObtenerCantidadProductosAsync(int carritoId)
        {
            using var connection = _context.CreateConnection();
            var cantidad = await connection.ExecuteScalarAsync<int>(
                "SELECT ISNULL(SUM(Cantidad), 0) FROM CarritoItem WHERE CarritoId = @CarritoId",
                new { CarritoId = carritoId }
            );
            return cantidad;
        }
    }
} 