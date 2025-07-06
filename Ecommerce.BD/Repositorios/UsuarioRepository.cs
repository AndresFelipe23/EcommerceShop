using System.Data;
using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;

namespace Ecommerce.BD.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DapperContext _context;

        public UsuarioRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> ListarUsuariosAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<dynamic>(
                "Usuario_List",
                commandType: CommandType.StoredProcedure
            );
            
            // Mapear manualmente para convertir DateTime a DateOnly
            var usuarios = new List<Usuario>();
            foreach (var item in result)
            {
                var usuario = new Usuario
                {
                    UsuId = item.UsuId,
                    UsuNombre = item.UsuNombre,
                    UsuApellido = item.UsuApellido,
                    UsuEmail = item.UsuEmail,
                    UsuPasswordHash = item.UsuPasswordHash,
                    UsuTelefono = item.UsuTelefono,
                    UsuGenero = item.UsuGenero,
                    UsuFechaNacimiento = item.UsuFechaNacimiento != null ? DateOnly.FromDateTime((DateTime)item.UsuFechaNacimiento) : null,
                    UsuImagenPerfil = item.UsuImagenPerfil,
                    UsuEstadoCuenta = item.UsuEstadoCuenta,
                    UsuFechaRegistro = item.UsuFechaRegistro,
                    UsuFechaUltimoLogin = item.UsuFechaUltimoLogin,
                    UsuProveedorAutenticacion = item.UsuProveedorAutenticacion,
                    UsuFechaActualizacion = item.UsuFechaActualizacion
                };
                usuarios.Add(usuario);
            }
            
            return usuarios;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "Usuario_GetById",
                new { UsuId = id },
                commandType: CommandType.StoredProcedure
            );
            
            if (result == null) return null;
            
            // Mapear manualmente para convertir DateTime a DateOnly
            var usuario = new Usuario
            {
                UsuId = result.UsuId,
                UsuNombre = result.UsuNombre,
                UsuApellido = result.UsuApellido,
                UsuEmail = result.UsuEmail,
                UsuPasswordHash = result.UsuPasswordHash,
                UsuTelefono = result.UsuTelefono,
                UsuGenero = result.UsuGenero,
                UsuFechaNacimiento = result.UsuFechaNacimiento != null ? DateOnly.FromDateTime((DateTime)result.UsuFechaNacimiento) : null,
                UsuImagenPerfil = result.UsuImagenPerfil,
                UsuEstadoCuenta = result.UsuEstadoCuenta,
                UsuFechaRegistro = result.UsuFechaRegistro,
                UsuFechaUltimoLogin = result.UsuFechaUltimoLogin,
                UsuProveedorAutenticacion = result.UsuProveedorAutenticacion,
                UsuFechaActualizacion = result.UsuFechaActualizacion
            };
            
            return usuario;
        }

        public async Task<int> InsertarUsuarioAsync(Usuario usuario)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UsuNombre", usuario.UsuNombre);
            parameters.Add("@UsuApellido", usuario.UsuApellido);
            parameters.Add("@UsuEmail", usuario.UsuEmail);
            parameters.Add("@UsuPasswordHash", usuario.UsuPasswordHash);
            parameters.Add("@UsuTelefono", usuario.UsuTelefono);
            parameters.Add("@UsuGenero", usuario.UsuGenero);
            parameters.Add("@UsuFechaNacimiento", usuario.UsuFechaNacimiento?.ToDateTime(TimeOnly.MinValue));
            parameters.Add("@UsuImagenPerfil", usuario.UsuImagenPerfil);
            parameters.Add("@UsuProveedorAutenticacion", usuario.UsuProveedorAutenticacion);
            parameters.Add("@NewUsuId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("Usuario_Insert", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@NewUsuId");
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UsuId", usuario.UsuId);
            parameters.Add("@UsuNombre", usuario.UsuNombre);
            parameters.Add("@UsuApellido", usuario.UsuApellido);
            parameters.Add("@UsuTelefono", usuario.UsuTelefono);
            parameters.Add("@UsuGenero", usuario.UsuGenero);
            parameters.Add("@UsuFechaNacimiento", usuario.UsuFechaNacimiento?.ToDateTime(TimeOnly.MinValue));
            parameters.Add("@UsuImagenPerfil", usuario.UsuImagenPerfil);
            parameters.Add("@UsuEstadoCuenta", usuario.UsuEstadoCuenta);

            var rows = await connection.ExecuteAsync("Usuario_Update", parameters, commandType: CommandType.StoredProcedure);

            return rows > 0;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(
                "Usuario_Delete",
                new { UsuId = id },
                commandType: CommandType.StoredProcedure
            );
            return rows > 0;
        }
    }
}
