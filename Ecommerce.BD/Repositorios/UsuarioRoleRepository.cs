using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class UsuarioRoleRepository : IUsuarioRoleRepository
    {
        private readonly DapperContext _context;

        public UsuarioRoleRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioRole>> ListarUsuarioRolesAsync()
        {
            var query = "UsuarioRole_List";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<UsuarioRole>(query, commandType: CommandType.StoredProcedure);
        }

        public async Task<UsuarioRole?> ObtenerPorIdAsync(int id)
        {
            var query = "UsuarioRole_ListPorId";
            var parameters = new { UsuarioRolId = id };
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UsuarioRole>(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertarUsuarioRoleAsync(UsuarioRole usuarioRol)
        {
            var query = "UsuarioRole_Insert";
            var parameters = new
            {
                usuarioRol.UsuId,
                usuarioRol.RolId
            };
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ActualizarUsuarioRoleAsync(UsuarioRole usuarioRol)
        {
            var query = "UsuarioRole_Update";
            var parameters = new
            {
                usuarioRol.UsuarioRolId,
                usuarioRol.UsuId,
                usuarioRol.RolId
            };
            using var connection = _context.CreateConnection();
            var filas = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            return filas > 0;
        }

        public async Task<bool> EliminarUsuarioRoleAsync(int id)
        {
            var query = "UsuarioRole_Delete";
            var parameters = new { UsuarioRolId = id };
            using var connection = _context.CreateConnection();
            var filas = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            return filas > 0;
        }
    }
}
