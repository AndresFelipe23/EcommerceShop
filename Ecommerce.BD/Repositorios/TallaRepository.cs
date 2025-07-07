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
            try
            {
                using var connection = _context.CreateConnection();
                Console.WriteLine($"TallaRepository: Intentando obtener talla con ID {id}");
                
                // Intentar primero con Tallas_ListPorId
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<Talla>(
                        "Tallas_ListPorId",
                        new { TallaId = id },
                        commandType: CommandType.StoredProcedure
                    );
                    
                    Console.WriteLine($"TallaRepository: Resultado obtenido con Tallas_ListPorId: {result?.TallaId ?? -1}");
                    return result;
                }
                catch (Exception spEx)
                {
                    Console.WriteLine($"TallaRepository: Error con Tallas_ListPorId: {spEx.Message}");
                    Console.WriteLine($"TallaRepository: Intentando con Tallas_List como fallback...");
                    
                    // Fallback: usar Tallas_List y filtrar
                    var allTallas = await connection.QueryAsync<Talla>("Tallas_List", commandType: CommandType.StoredProcedure);
                    var result = allTallas.FirstOrDefault(t => t.TallaId == id);
                    
                    Console.WriteLine($"TallaRepository: Resultado obtenido con fallback: {result?.TallaId ?? -1}");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TallaRepository: Error al obtener talla con ID {id}: {ex.Message}");
                Console.WriteLine($"TallaRepository: Stack trace: {ex.StackTrace}");
                throw;
            }
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