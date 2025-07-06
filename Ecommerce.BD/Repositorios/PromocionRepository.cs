using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class PromocionRepository : IPromocionRepository
    {
        private readonly DapperContext _context;

        public PromocionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Promocione>> ListarPromocionesAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Promocione>("Promocion_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Promocione?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Promocione>(
                "Promocion_ListPorId",
                new { PromocionId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarPromocionAsync(Promocione promocion)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "Promocion_Insert",
                new { 
                    promocion.Nombre, 
                    promocion.Descripcion, 
                    promocion.TipoDescuento, 
                    promocion.ValorDescuento, 
                    promocion.FechaInicio, 
                    promocion.FechaFin, 
                    promocion.Activo 
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarPromocionAsync(Promocione promocion)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Promocion_Update",
                new { 
                    promocion.PromocionId, 
                    promocion.Nombre, 
                    promocion.Descripcion, 
                    promocion.TipoDescuento, 
                    promocion.ValorDescuento, 
                    promocion.FechaInicio, 
                    promocion.FechaFin, 
                    promocion.Activo 
                },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarPromocionAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Promocion_Delete",
                new { PromocionId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Promocione>> ListarPromocionesActivasAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Promocione>(
                "Promocion_ListActivas",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Promocione>> ListarPromocionesPorFechaAsync(DateTime fecha)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Promocione>(
                "Promocion_ListPorFecha",
                new { Fecha = fecha },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Promocione>> ListarPromocionesPorTipoAsync(string tipoDescuento)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Promocione>(
                "Promocion_ListPorTipo",
                new { TipoDescuento = tipoDescuento },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
} 