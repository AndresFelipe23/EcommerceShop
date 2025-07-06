using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly DapperContext _context;

        public InventarioRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventario>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Inventario>("Inventario_List");
        }

        public async Task<Inventario?> ObtenerPorIdAsync(int inventarioId)
        {
            using var connection = _context.CreateConnection();
            var inventarios = await connection.QueryAsync<Inventario>(
                "SELECT * FROM Inventario WHERE InventarioId = @InventarioId",
                new { InventarioId = inventarioId }
            );
            return inventarios.FirstOrDefault();
        }

        public async Task<IEnumerable<Inventario>> ListarPorProductoAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Inventario>(
                "Inventario_ListPorProducto",
                new { ProId = proId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Inventario>> ListarPorProductoTallaColorAsync(int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Inventario>(
                "SELECT * FROM Inventario WHERE ProductoTallaColorId = @ProductoTallaColorId ORDER BY FechaMovimiento DESC",
                new { ProductoTallaColorId = productoTallaColorId }
            );
        }

        public async Task<int> CrearAsync(Inventario inventario)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductoTallaColorId", inventario.ProductoTallaColorId);
            parameters.Add("@TipoMovimiento", inventario.TipoMovimiento);
            parameters.Add("@Cantidad", inventario.Cantidad);
            parameters.Add("@Descripcion", inventario.Descripcion);
            parameters.Add("@UsuId", inventario.UsuId);
            parameters.Add("@InventarioId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "Inventario_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@InventarioId");
        }

        public async Task<bool> ActualizarAsync(Inventario inventario)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@InventarioId", inventario.InventarioId);
            parameters.Add("@Cantidad", inventario.Cantidad);

            var rowsAffected = await connection.ExecuteAsync(
                "Inventario_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int inventarioId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "Inventario_Delete",
                new { InventarioId = inventarioId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int inventarioId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Inventario WHERE InventarioId = @InventarioId",
                new { InventarioId = inventarioId }
            );
            return count > 0;
        }

        public async Task<decimal> ObtenerStockActualAsync(int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            
            // Calcular el stock actual basado en los movimientos
            var stock = await connection.ExecuteScalarAsync<decimal>(
                @"SELECT ISNULL(SUM(
                    CASE 
                        WHEN TipoMovimiento IN ('Entrada', 'Ajuste') THEN Cantidad
                        WHEN TipoMovimiento IN ('Salida', 'Venta') THEN -Cantidad
                        ELSE 0
                    END
                ), 0) 
                FROM Inventario 
                WHERE ProductoTallaColorId = @ProductoTallaColorId",
                new { ProductoTallaColorId = productoTallaColorId }
            );
            
            return stock;
        }

        public async Task<bool> RegistrarMovimientoAsync(int productoTallaColorId, string tipoMovimiento, int cantidad, string descripcion, int? usuId = null)
        {
            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Crear el registro de inventario
                var inventario = new Inventario
                {
                    ProductoTallaColorId = productoTallaColorId,
                    TipoMovimiento = tipoMovimiento,
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    UsuId = usuId
                };

                var inventarioId = await CrearAsync(inventario);

                // Actualizar el stock en ProductoTallaColor si es necesario
                if (tipoMovimiento == "Entrada" || tipoMovimiento == "Salida" || tipoMovimiento == "Venta")
                {
                    var stockActual = await ObtenerStockActualAsync(productoTallaColorId);
                    
                    await connection.ExecuteAsync(
                        "UPDATE ProductoTallaColor SET Stock = @Stock WHERE ProductoTallaColorId = @ProductoTallaColorId",
                        new { Stock = stockActual, ProductoTallaColorId = productoTallaColorId },
                        transaction
                    );
                }

                transaction.Commit();
                return inventarioId > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Inventario>> ListarPorTipoMovimientoAsync(string tipoMovimiento)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Inventario>(
                "SELECT * FROM Inventario WHERE TipoMovimiento = @TipoMovimiento ORDER BY FechaMovimiento DESC",
                new { TipoMovimiento = tipoMovimiento }
            );
        }

        public async Task<IEnumerable<Inventario>> ListarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Inventario>(
                "SELECT * FROM Inventario WHERE FechaMovimiento BETWEEN @FechaInicio AND @FechaFin ORDER BY FechaMovimiento DESC",
                new { FechaInicio = fechaInicio, FechaFin = fechaFin }
            );
        }

        public async Task<IEnumerable<Inventario>> ListarPorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Inventario>(
                "SELECT * FROM Inventario WHERE UsuId = @UsuId ORDER BY FechaMovimiento DESC",
                new { UsuId = usuId }
            );
        }
    }
} 