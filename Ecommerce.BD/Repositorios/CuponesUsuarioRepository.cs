using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class CuponesUsuarioRepository : ICuponesUsuarioRepository
    {
        private readonly DapperContext _context;

        public CuponesUsuarioRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CuponesUsuario>> ListarCuponesUsuariosAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<CuponesUsuario>("CuponesUsuario_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<CuponesUsuario?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<CuponesUsuario>(
                "CuponesUsuario_ListPorId",
                new { CuponUsuarioId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarCuponesUsuarioAsync(CuponesUsuario cuponUsuario)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "CuponesUsuario_Insert",
                new { 
                    cuponUsuario.CuponId, 
                    cuponUsuario.UsuId, 
                    cuponUsuario.Usado, 
                    cuponUsuario.FechaUso 
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarCuponesUsuarioAsync(CuponesUsuario cuponUsuario)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "CuponesUsuario_Update",
                new { 
                    cuponUsuario.CuponUsuarioId, 
                    cuponUsuario.CuponId, 
                    cuponUsuario.UsuId, 
                    cuponUsuario.Usado, 
                    cuponUsuario.FechaUso 
                },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarCuponesUsuarioAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "CuponesUsuario_Delete",
                new { CuponUsuarioId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<IEnumerable<CuponesUsuario>> ListarCuponesPorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<CuponesUsuario>(
                "CuponesUsuario_ListPorUsuario",
                new { UsuId = usuId },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<CuponesUsuario>> ListarCuponesUsadosPorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<CuponesUsuario>(
                "CuponesUsuario_ListUsadosPorUsuario",
                new { UsuId = usuId },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<CuponesUsuario>> ListarCuponesDisponiblesPorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<CuponesUsuario>(
                "CuponesUsuario_ListDisponiblesPorUsuario",
                new { UsuId = usuId },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<CuponesUsuario?> ObtenerCuponUsuarioAsync(int cuponId, int usuId)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<CuponesUsuario>(
                "CuponesUsuario_ListPorCuponYUsuario",
                new { CuponId = cuponId, UsuId = usuId },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> MarcarCuponComoUsadoAsync(int cuponUsuarioId)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "CuponesUsuario_MarcarComoUsado",
                new { 
                    CuponUsuarioId = cuponUsuarioId,
                    FechaUso = DateTime.Now
                },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> AsignarCuponAUsuarioAsync(int cuponId, int usuId)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "CuponesUsuario_AsignarCupon",
                new { 
                    CuponId = cuponId, 
                    UsuId = usuId,
                    Usado = false
                },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }
    }
} 