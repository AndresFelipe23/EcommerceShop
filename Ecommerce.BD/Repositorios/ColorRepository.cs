using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class ColorRepository : IColorRepository
    {
        private readonly DapperContext _context;

        public ColorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Colore>> ListarColoresAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Colore>("Color_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Colore?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Colore>(
                "Color_ListPorId",
                new { ColorId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarColorAsync(Colore color)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "Color_Insert",
                new { color.ColNombre, color.ColCodigoHex },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarColorAsync(Colore color)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Color_Update",
                new { color.ColorId, color.ColNombre, color.ColCodigoHex },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarColorAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Color_Delete",
                new { ColorId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<Colore?> ObtenerPorNombreAsync(string nombre)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Colore>(
                "Color_ListPorNombre",
                new { ColNombre = nombre },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<Colore?> ObtenerPorCodigoHexAsync(string codigoHex)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Colore>(
                "Color_ListPorCodigoHex",
                new { ColCodigoHex = codigoHex },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
} 