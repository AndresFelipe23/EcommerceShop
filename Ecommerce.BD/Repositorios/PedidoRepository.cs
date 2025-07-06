using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly DapperContext _context;

        public PedidoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Pedido>("Pedidos_List");
        }

        public async Task<Pedido?> ObtenerPorIdAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            var pedidos = await connection.QueryAsync<Pedido>(
                "SELECT * FROM Pedidos WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId }
            );
            return pedidos.FirstOrDefault();
        }

        public async Task<IEnumerable<Pedido>> ListarPorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Pedido>(
                "SELECT * FROM Pedidos WHERE UsuId = @UsuId ORDER BY FechaPedido DESC",
                new { UsuId = usuId }
            );
        }

        public async Task<IEnumerable<Pedido>> ListarPorEstadoAsync(string estado)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Pedido>(
                "SELECT * FROM Pedidos WHERE Estado = @Estado ORDER BY FechaPedido DESC",
                new { Estado = estado }
            );
        }

        public async Task<IEnumerable<Pedido>> ListarPorFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Pedido>(
                "SELECT * FROM Pedidos WHERE FechaPedido BETWEEN @FechaInicio AND @FechaFin ORDER BY FechaPedido DESC",
                new { FechaInicio = fechaInicio, FechaFin = fechaFin }
            );
        }

        public async Task<int> CrearAsync(Pedido pedido)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@UsuId", pedido.UsuId);
            parameters.Add("@DireccionEnvioId", pedido.DireccionEnvioId);
            parameters.Add("@Estado", pedido.Estado);
            parameters.Add("@MetodoPago", pedido.MetodoPago);
            parameters.Add("@Total", pedido.Total);
            parameters.Add("@Observaciones", pedido.Observaciones);
            parameters.Add("@PedidoId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "Pedidos_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@PedidoId");
        }

        public async Task<bool> ActualizarAsync(Pedido pedido)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@PedidoId", pedido.PedidoId);
            parameters.Add("@Estado", pedido.Estado);
            parameters.Add("@MetodoPago", pedido.MetodoPago);
            parameters.Add("@Total", pedido.Total);
            parameters.Add("@Observaciones", pedido.Observaciones);

            var rowsAffected = await connection.ExecuteAsync(
                "Pedidos_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "Pedidos_Delete",
                new { PedidoId = pedidoId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Pedidos WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId }
            );
            return count > 0;
        }

        public async Task<bool> ActualizarEstadoAsync(int pedidoId, string estado)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE Pedidos SET Estado = @Estado, FechaActualizacion = GETDATE() WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId, Estado = estado }
            );

            return rowsAffected > 0;
        }

        public async Task<decimal> CalcularTotalAsync(int pedidoId)
        {
            using var connection = _context.CreateConnection();
            var total = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(Subtotal), 0) FROM PedidoDetalle WHERE PedidoId = @PedidoId",
                new { PedidoId = pedidoId }
            );
            return total;
        }

        public async Task<IEnumerable<Pedido>> ListarPorMetodoPagoAsync(string metodoPago)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Pedido>(
                "SELECT * FROM Pedidos WHERE MetodoPago = @MetodoPago ORDER BY FechaPedido DESC",
                new { MetodoPago = metodoPago }
            );
        }

        public async Task<object> ObtenerEstadisticasAsync()
        {
            using var connection = _context.CreateConnection();
            
            var estadisticas = await connection.QueryFirstOrDefaultAsync(@"
                SELECT 
                    COUNT(*) AS TotalPedidos,
                    COUNT(CASE WHEN Estado = 'Pendiente' THEN 1 END) AS PedidosPendientes,
                    COUNT(CASE WHEN Estado = 'Enviado' THEN 1 END) AS PedidosEnviados,
                    COUNT(CASE WHEN Estado = 'Entregado' THEN 1 END) AS PedidosEntregados,
                    COUNT(CASE WHEN Estado = 'Cancelado' THEN 1 END) AS PedidosCancelados,
                    ISNULL(SUM(Total), 0) AS TotalVentas,
                    ISNULL(AVG(Total), 0) AS PromedioVentas
                FROM Pedidos
                WHERE Estado != 'Cancelado'");

            return estadisticas ?? new { 
                TotalPedidos = 0, 
                PedidosPendientes = 0, 
                PedidosEnviados = 0, 
                PedidosEntregados = 0, 
                PedidosCancelados = 0, 
                TotalVentas = 0m, 
                PromedioVentas = 0m 
            };
        }

        public async Task<IEnumerable<Pedido>> ListarPedidosRecientesAsync(int cantidad = 10)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Pedido>(
                $"SELECT TOP {cantidad} * FROM Pedidos ORDER BY FechaPedido DESC"
            );
        }

        public async Task<bool> VerificarPedidoUsuarioAsync(int pedidoId, int usuId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Pedidos WHERE PedidoId = @PedidoId AND UsuId = @UsuId",
                new { PedidoId = pedidoId, UsuId = usuId }
            );
            return count > 0;
        }
    }
} 