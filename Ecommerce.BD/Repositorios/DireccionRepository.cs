using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class DireccionRepository : IDireccionRepository
    {
        private readonly DapperContext _context;

        public DireccionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Direccione>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Direccione>("Direccion_List");
        }

        public async Task<Direccione?> ObtenerPorIdAsync(int dirId)
        {
            using var connection = _context.CreateConnection();
            var direcciones = await connection.QueryAsync<Direccione>(
                "Direccion_ListPorId",
                new { DirId = dirId },
                commandType: CommandType.StoredProcedure
            );
            return direcciones.FirstOrDefault();
        }

        public async Task<IEnumerable<Direccione>> ListarPorUsuarioAsync(int usuId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Direccione>(
                "Direccion_ListPorUsuario",
                new { UsuId = usuId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CrearAsync(Direccione direccion)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@UsuId", direccion.UsuId);
            parameters.Add("@DirTitulo", direccion.DirTitulo);
            parameters.Add("@DirNombre", direccion.DirNombre);
            parameters.Add("@DirTelefono", direccion.DirTelefono);
            parameters.Add("@DirPais", direccion.DirPais);
            parameters.Add("@DirDepartamento", direccion.DirDepartamento);
            parameters.Add("@DirCiudad", direccion.DirCiudad);
            parameters.Add("@DirCodigoPostal", direccion.DirCodigoPostal);
            parameters.Add("@DirLinea1", direccion.DirLinea1);
            parameters.Add("@DirLinea2", direccion.DirLinea2);
            parameters.Add("@DirReferencia", direccion.DirReferencia);
            parameters.Add("@DirEsPrincipal", direccion.DirEsPrincipal);
            parameters.Add("@DirId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "Direccion_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@DirId");
        }

        public async Task<bool> ActualizarAsync(Direccione direccion)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@DirId", direccion.DirId);
            parameters.Add("@DirTitulo", direccion.DirTitulo);
            parameters.Add("@DirNombre", direccion.DirNombre);
            parameters.Add("@DirTelefono", direccion.DirTelefono);
            parameters.Add("@DirPais", direccion.DirPais);
            parameters.Add("@DirDepartamento", direccion.DirDepartamento);
            parameters.Add("@DirCiudad", direccion.DirCiudad);
            parameters.Add("@DirCodigoPostal", direccion.DirCodigoPostal);
            parameters.Add("@DirLinea1", direccion.DirLinea1);
            parameters.Add("@DirLinea2", direccion.DirLinea2);
            parameters.Add("@DirReferencia", direccion.DirReferencia);
            parameters.Add("@DirEsPrincipal", direccion.DirEsPrincipal);

            var rowsAffected = await connection.ExecuteAsync(
                "Direccion_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int dirId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "Direccion_Delete",
                new { DirId = dirId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int dirId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Direcciones WHERE DirId = @DirId",
                new { DirId = dirId }
            );
            return count > 0;
        }

        public async Task<bool> EsPrincipalAsync(int dirId)
        {
            using var connection = _context.CreateConnection();
            var esPrincipal = await connection.ExecuteScalarAsync<bool>(
                "SELECT DirEsPrincipal FROM Direcciones WHERE DirId = @DirId",
                new { DirId = dirId }
            );
            return esPrincipal;
        }

        public async Task<bool> MarcarComoPrincipalAsync(int dirId, int usuId)
        {
            using var connection = _context.CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Primero, desmarcar todas las direcciones del usuario como no principales
                await connection.ExecuteAsync(
                    "UPDATE Direcciones SET DirEsPrincipal = 0 WHERE UsuId = @UsuId",
                    new { UsuId = usuId },
                    transaction
                );

                // Luego, marcar la dirección específica como principal
                var rowsAffected = await connection.ExecuteAsync(
                    "UPDATE Direcciones SET DirEsPrincipal = 1 WHERE DirId = @DirId AND UsuId = @UsuId",
                    new { DirId = dirId, UsuId = usuId },
                    transaction
                );

                transaction.Commit();
                return rowsAffected > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
} 