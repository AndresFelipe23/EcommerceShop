using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class TemporadaRepository : ITemporadaRepository
    {
        private readonly DapperContext _context;

        public TemporadaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Temporada>> ListarTemporadasAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Temporada>("Temporada_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Temporada?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Temporada>(
                "Temporada_ListPorId",
                new { TemporadaId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarTemporadaAsync(Temporada temporada)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "Temporada_Insert",
                new { 
                    temporada.Nombre, 
                    temporada.Descripcion, 
                    temporada.FechaInicio, 
                    temporada.FechaFin, 
                    temporada.Activo,
                    FechaCreacion = DateTime.Now
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarTemporadaAsync(Temporada temporada)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Temporada_Update",
                new { 
                    temporada.TemporadaId, 
                    temporada.Nombre, 
                    temporada.Descripcion, 
                    temporada.FechaInicio, 
                    temporada.FechaFin, 
                    temporada.Activo 
                },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarTemporadaAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Temporada_Delete",
                new { TemporadaId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Temporada>> ListarTemporadasActivasAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Temporada>(
                "Temporada_ListActivas",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Temporada>> ListarTemporadasPorFechaAsync(DateTime fecha)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Temporada>(
                "Temporada_ListPorFecha",
                new { Fecha = fecha },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
} 