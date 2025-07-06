using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class TallaRepository : ITallaRepository
    {
        private readonly DapperContext _context;

        public TallaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Talla>> ListarTallasAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Talla>("Tallas_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Talla?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Talla>(
                "Tallas_ListPorId",
                new { TallaId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarTallaAsync(Talla talla)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "Tallas_Insert",
                new { talla.TalNombre, talla.TalGenero, talla.TalOrdenVisualizacion },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarTallaAsync(Talla talla)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Tallas_Update",
                new { talla.TallaId, talla.TalNombre, talla.TalGenero, talla.TalOrdenVisualizacion },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarTallaAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Tallas_Delete",
                new { TallaId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Talla>> ListarTallasPorGeneroAsync(string genero)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Talla>(
                "Tallas_ListPorGenero",
                new { TalGenero = genero },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
} 