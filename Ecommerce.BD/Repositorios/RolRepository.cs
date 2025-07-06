using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class RolRepository : IRolRepository
    {
        private readonly DapperContext _context;

        public RolRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> ListarRolesAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Role>("Roles_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Role?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Role>(
                "Roles_ListPorId",
                new { RolId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarRolAsync(Role rol)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "Roles_Insert",
                new { rol.RolNombre },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarRolAsync(Role rol)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Roles_Update",
                new { rol.RolId, rol.RolNombre },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarRolAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Roles_Delete",
                new { RolId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }
    }
}
