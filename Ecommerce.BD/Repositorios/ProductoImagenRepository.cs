using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class ProductoImagenRepository : IProductoImagenRepository
    {
        private readonly DapperContext _context;

        public ProductoImagenRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoImagene>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoImagene>("ProductoImagenes_List");
        }

        public async Task<ProductoImagene?> ObtenerPorIdAsync(int imgId)
        {
            using var connection = _context.CreateConnection();
            var imagenes = await connection.QueryAsync<ProductoImagene>(
                "SELECT * FROM ProductoImagenes WHERE ImgId = @ImgId",
                new { ImgId = imgId }
            );
            return imagenes.FirstOrDefault();
        }

        public async Task<IEnumerable<ProductoImagene>> ListarPorProductoAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoImagene>(
                "ProductoImagenes_ListPorProducto",
                new { ProId = proId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CrearAsync(ProductoImagene productoImagen)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProId", productoImagen.ProId);
            parameters.Add("@ImgUrl", productoImagen.ImgUrl);
            parameters.Add("@ImgOrden", productoImagen.ImgOrden);
            parameters.Add("@ImgAlt", productoImagen.ImgAlt);
            parameters.Add("@ImgId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "ProductoImagenes_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ImgId");
        }

        public async Task<bool> ActualizarAsync(ProductoImagene productoImagen)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ImgId", productoImagen.ImgId);
            parameters.Add("@ImgUrl", productoImagen.ImgUrl);
            parameters.Add("@ImgOrden", productoImagen.ImgOrden);
            parameters.Add("@ImgAlt", productoImagen.ImgAlt);

            var rowsAffected = await connection.ExecuteAsync(
                "ProductoImagenes_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int imgId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "ProductoImagenes_Delete",
                new { ImgId = imgId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int imgId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM ProductoImagenes WHERE ImgId = @ImgId",
                new { ImgId = imgId }
            );
            return count > 0;
        }

        public async Task<bool> EliminarPorProductoAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "DELETE FROM ProductoImagenes WHERE ProId = @ProId",
                new { ProId = proId }
            );

            return rowsAffected > 0;
        }

        public async Task<int> ObtenerCantidadImagenesAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var cantidad = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM ProductoImagenes WHERE ProId = @ProId",
                new { ProId = proId }
            );
            return cantidad;
        }

        public async Task<ProductoImagene?> ObtenerImagenPrincipalAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var imagenes = await connection.QueryAsync<ProductoImagene>(
                "SELECT * FROM ProductoImagenes WHERE ProId = @ProId AND ImgOrden = 0",
                new { ProId = proId }
            );
            return imagenes.FirstOrDefault();
        }

        public async Task<bool> ActualizarOrdenAsync(int imgId, int nuevoOrden)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE ProductoImagenes SET ImgOrden = @NuevoOrden WHERE ImgId = @ImgId",
                new { ImgId = imgId, NuevoOrden = nuevoOrden }
            );

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<ProductoImagene>> ListarPorOrdenAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoImagene>(
                "SELECT * FROM ProductoImagenes WHERE ProId = @ProId ORDER BY ImgOrden ASC",
                new { ProId = proId }
            );
        }
    }
} 