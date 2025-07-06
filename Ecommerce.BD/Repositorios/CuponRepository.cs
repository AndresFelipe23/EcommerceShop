using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class CuponRepository : ICuponRepository
    {
        private readonly DapperContext _context;

        public CuponRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cupone>> ListarCuponesAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Cupone>("Cupon_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Cupone?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Cupone>(
                "Cupon_ListPorId",
                new { CuponId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarCuponAsync(Cupone cupon)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "Cupon_Insert",
                new { 
                    cupon.Codigo, 
                    cupon.Descripcion, 
                    cupon.TipoDescuento, 
                    cupon.ValorDescuento, 
                    cupon.MontoMinimo, 
                    cupon.FechaInicio, 
                    cupon.FechaFin, 
                    cupon.LimiteUso, 
                    cupon.UsosRealizados, 
                    cupon.Activo 
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarCuponAsync(Cupone cupon)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Cupon_Update",
                new { 
                    cupon.CuponId, 
                    cupon.Codigo, 
                    cupon.Descripcion, 
                    cupon.TipoDescuento, 
                    cupon.ValorDescuento, 
                    cupon.MontoMinimo, 
                    cupon.FechaInicio, 
                    cupon.FechaFin, 
                    cupon.LimiteUso, 
                    cupon.UsosRealizados, 
                    cupon.Activo 
                },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarCuponAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Cupon_Delete",
                new { CuponId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<Cupone?> ObtenerPorCodigoAsync(string codigo)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Cupone>(
                "Cupon_ListPorCodigo",
                new { Codigo = codigo },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Cupone>> ListarCuponesActivosAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Cupone>(
                "Cupon_ListActivos",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Cupone>> ListarCuponesPorFechaAsync(DateTime fecha)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Cupone>(
                "Cupon_ListPorFecha",
                new { Fecha = fecha },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Cupone>> ListarCuponesPorTipoAsync(string tipoDescuento)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Cupone>(
                "Cupon_ListPorTipo",
                new { TipoDescuento = tipoDescuento },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ValidarCuponAsync(string codigo, decimal montoCompra)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<bool>(
                "Cupon_Validar",
                new { Codigo = codigo, MontoCompra = montoCompra },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> IncrementarUsoCuponAsync(int cuponId)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Cupon_IncrementarUso",
                new { CuponId = cuponId },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }
    }
} 