using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class LogRepository : ILogRepository
    {
        private readonly DapperContext _context;

        public LogRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Log>> ListarLogsAsync()
        {
            var query = "Log_List";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Log>(query, commandType: CommandType.StoredProcedure);
        }

        public async Task<Log?> ObtenerPorIdAsync(int id)
        {
            var query = "Log_ListPorId";
            var parameters = new DynamicParameters();
            parameters.Add("@LogId", id);

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Log>(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertarLogAsync(Log log)
        {
            var query = "Log_Insert";
            var parameters = new DynamicParameters();
            parameters.Add("@UsuId", log.UsuId);
            parameters.Add("@Tipo", log.Tipo);
            parameters.Add("@Mensaje", log.Mensaje);
            parameters.Add("@Origen", log.Origen);
            parameters.Add("@Metodo", log.Metodo);
            parameters.Add("@DatosEntrada", log.DatosEntrada);
            parameters.Add("@DatosSalida", log.DatosSalida);
            parameters.Add("@IP", log.Ip);
            parameters.Add("@Navegador", log.Navegador);

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> EliminarLogAsync(int id)
        {
            var query = "Log_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("@LogId", id);

            using var connection = _context.CreateConnection();
            var filas = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            return filas > 0;
        }
    }
}
