using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly DapperContext _context;

        public CarritoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Carrito>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Carrito>("Carrito_List");
        }

        public async Task<Carrito?> ObtenerPorIdAsync(int carritoId)
        {
            using var connection = _context.CreateConnection();
            var carritos = await connection.QueryAsync<Carrito>(
                "SELECT * FROM Carrito WHERE CarritoId = @CarritoId",
                new { CarritoId = carritoId }
            );
            return carritos.FirstOrDefault();
        }

        public async Task<Carrito?> ObtenerPorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            var carritos = await connection.QueryAsync<Carrito>(
                "Carrito_ListPorUsuario",
                new { UsuId = usuId },
                commandType: CommandType.StoredProcedure
            );
            return carritos.FirstOrDefault();
        }

        public async Task<int> CrearAsync(Carrito carrito)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@UsuId", carrito.UsuId);
            parameters.Add("@CarritoId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "Carrito_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@CarritoId");
        }

        public async Task<bool> ActualizarAsync(Carrito carrito)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@CarritoId", carrito.CarritoId);
            parameters.Add("@UsuId", carrito.UsuId);

            var rowsAffected = await connection.ExecuteAsync(
                "Carrito_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int carritoId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "Carrito_Delete",
                new { CarritoId = carritoId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int carritoId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Carrito WHERE CarritoId = @CarritoId",
                new { CarritoId = carritoId }
            );
            return count > 0;
        }

        public async Task<bool> ExistePorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Carrito WHERE UsuId = @UsuId",
                new { UsuId = usuId }
            );
            return count > 0;
        }

        public async Task<bool> VaciarCarritoAsync(int carritoId)
        {
            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Eliminar todos los items del carrito
                var rowsAffected = await connection.ExecuteAsync(
                    "DELETE FROM CarritoItem WHERE CarritoId = @CarritoId",
                    new { CarritoId = carritoId },
                    transaction
                );

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<decimal> CalcularTotalAsync(int carritoId)
        {
            using var connection = _context.CreateConnection();
            var total = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM CarritoItem WHERE CarritoId = @CarritoId",
                new { CarritoId = carritoId }
            );
            return total;
        }
    }
} 