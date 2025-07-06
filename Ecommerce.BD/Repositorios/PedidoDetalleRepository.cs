using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class PedidoDetalleRepository : IPedidoDetalleRepository
    {
        private readonly DapperContext _context;

        public PedidoDetalleRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDetalle>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PedidoDetalle>("PedidoDetalle_List");
        }

        public async Task<PedidoDetalle?> ObtenerPorIdAsync(int pedidoDetalleId)
        {
            using var connection = _context.CreateConnection();
            var detalles = await connection.QueryAsync<PedidoDetalle>(
                "SELECT * FROM PedidoDetalle WHERE PedidoDetalleId = @PedidoDetalleId",
                new { PedidoDetalleId = pedidoDetalleId }
            );
            return detalles.FirstOrDefault();
        }

        public async Task<IEnumerable<PedidoDetalle>> ListarPorPedidoAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PedidoDetalle>(
                "PedidoDetalle_ListPorPedidoId",
                new { PedidoId = pedidoId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CrearAsync(PedidoDetalle pedidoDetalle)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@PedidoId", pedidoDetalle.PedidoId);
            parameters.Add("@ProductoTallaColorId", pedidoDetalle.ProductoTallaColorId);
            parameters.Add("@Cantidad", pedidoDetalle.Cantidad);
            parameters.Add("@PrecioUnitario", pedidoDetalle.PrecioUnitario);
            parameters.Add("@PedidoDetalleId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "PedidoDetalle_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@PedidoDetalleId");
        }

        public async Task<bool> ActualizarAsync(PedidoDetalle pedidoDetalle)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE PedidoDetalle SET Cantidad = @Cantidad, PrecioUnitario = @PrecioUnitario WHERE PedidoDetalleId = @PedidoDetalleId",
                new { 
                    PedidoDetalleId = pedidoDetalle.PedidoDetalleId,
                    Cantidad = pedidoDetalle.Cantidad,
                    PrecioUnitario = pedidoDetalle.PrecioUnitario
                }
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int pedidoDetalleId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM PedidoDetalle WHERE PedidoDetalleId = @PedidoDetalleId",
                new { PedidoDetalleId = pedidoDetalleId }
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int pedidoDetalleId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM PedidoDetalle WHERE PedidoDetalleId = @PedidoDetalleId",
                new { PedidoDetalleId = pedidoDetalleId }
            );
            return count > 0;
        }

        public async Task<bool> EliminarPorPedidoAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM PedidoDetalle WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId }
            );

            return rowsAffected > 0;
        }

        public async Task<decimal> CalcularSubtotalAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            var subtotal = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(Subtotal), 0) FROM PedidoDetalle WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId }
            );
            return subtotal;
        }

        public async Task<int> ObtenerCantidadProductosAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            var cantidad = await connection.ExecuteScalarAsync<int>(
                "SELECT ISNULL(SUM(Cantidad), 0) FROM PedidoDetalle WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId }
            );
            return cantidad;
        }

        public async Task<IEnumerable<PedidoDetalle>> ListarPorProductoAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PedidoDetalle>(
                @"SELECT pd.* FROM PedidoDetalle pd
                  INNER JOIN ProductoTallaColor ptc ON pd.ProductoTallaColorId = ptc.ProductoTallaColorId
                  WHERE ptc.ProId = @ProId
                  ORDER BY pd.PedidoDetalleId DESC",
                new { ProId = proId }
            );
        }

        public async Task<bool> VerificarProductoEnPedidoAsync(int pedidoId, int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM PedidoDetalle WHERE PedidoId = @PedidoId AND ProductoTallaColorId = @ProductoTallaColorId",
                new { PedidoId = pedidoId, ProductoTallaColorId = productoTallaColorId }
            );
            return count > 0;
        }
    }
} 