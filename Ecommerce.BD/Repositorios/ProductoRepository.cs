using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly DapperContext _context;

        public ProductoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>("Productos_List");
        }

        public async Task<Producto?> ObtenerPorIdAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var productos = await connection.QueryAsync<Producto>(
                "Productos_ListPorId",
                new { ProId = proId },
                commandType: CommandType.StoredProcedure
            );
            return productos.FirstOrDefault();
        }

        public async Task<IEnumerable<Producto>> ListarPorCategoriaAsync(int catId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>(
                "SELECT * FROM Productos WHERE ProCategoriaId = @CatId AND ProActivo = 1 ORDER BY ProFechaCreacion DESC",
                new { CatId = catId }
            );
        }

        public async Task<IEnumerable<Producto>> ListarPorGeneroAsync(string genero)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>(
                "SELECT * FROM Productos WHERE ProGenero = @Genero AND ProActivo = 1 ORDER BY ProFechaCreacion DESC",
                new { Genero = genero }
            );
        }

        public async Task<IEnumerable<Producto>> ListarActivosAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>(
                "SELECT * FROM Productos WHERE ProActivo = 1 ORDER BY ProFechaCreacion DESC"
            );
        }

        public async Task<int> CrearAsync(Producto producto)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProNombre", producto.ProNombre);
            parameters.Add("@ProDescripcion", producto.ProDescripcion);
            parameters.Add("@ProPrecio", producto.ProPrecio);
            parameters.Add("@ProImagenPrincipal", producto.ProImagenPrincipal);
            parameters.Add("@ProGenero", producto.ProGenero);
            parameters.Add("@ProCategoriaId", producto.ProCategoriaId);
            parameters.Add("@ProId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "Productos_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ProId");
        }

        public async Task<bool> ActualizarAsync(Producto producto)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProId", producto.ProId);
            parameters.Add("@ProNombre", producto.ProNombre);
            parameters.Add("@ProDescripcion", producto.ProDescripcion);
            parameters.Add("@ProPrecio", producto.ProPrecio);
            parameters.Add("@ProImagenPrincipal", producto.ProImagenPrincipal);
            parameters.Add("@ProGenero", producto.ProGenero);
            parameters.Add("@ProCategoriaId", producto.ProCategoriaId);
            parameters.Add("@ProActivo", producto.ProActivo);

            var rowsAffected = await connection.ExecuteAsync(
                "Productos_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "Productos_Delete",
                new { ProId = proId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Productos WHERE ProId = @ProId",
                new { ProId = proId }
            );
            return count > 0;
        }

        public async Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>(
                "SELECT * FROM Productos WHERE ProNombre LIKE @Nombre AND ProActivo = 1 ORDER BY ProNombre",
                new { Nombre = $"%{nombre}%" }
            );
        }

        public async Task<IEnumerable<Producto>> ListarPorRangoPrecioAsync(decimal precioMin, decimal precioMax)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>(
                "SELECT * FROM Productos WHERE ProPrecio BETWEEN @PrecioMin AND @PrecioMax AND ProActivo = 1 ORDER BY ProPrecio",
                new { PrecioMin = precioMin, PrecioMax = precioMax }
            );
        }

        public async Task<IEnumerable<Producto>> ListarProductosRecientesAsync(int cantidad = 10)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>(
                $"SELECT TOP {cantidad} * FROM Productos WHERE ProActivo = 1 ORDER BY ProFechaCreacion DESC"
            );
        }

        public async Task<object> ObtenerEstadisticasAsync()
        {
            using var connection = _context.CreateConnection();
            
            var estadisticas = await connection.QueryFirstOrDefaultAsync(@"
                SELECT 
                    COUNT(*) AS TotalProductos,
                    COUNT(CASE WHEN ProActivo = 1 THEN 1 END) AS ProductosActivos,
                    COUNT(CASE WHEN ProActivo = 0 THEN 1 END) AS ProductosInactivos,
                    ISNULL(AVG(ProPrecio), 0) AS PrecioPromedio,
                    ISNULL(MIN(ProPrecio), 0) AS PrecioMinimo,
                    ISNULL(MAX(ProPrecio), 0) AS PrecioMaximo,
                    COUNT(DISTINCT ProCategoriaId) AS CategoriasUtilizadas,
                    COUNT(DISTINCT ProGenero) AS GenerosUtilizados
                FROM Productos");

            return estadisticas ?? new { 
                TotalProductos = 0, 
                ProductosActivos = 0, 
                ProductosInactivos = 0, 
                PrecioPromedio = 0m, 
                PrecioMinimo = 0m, 
                PrecioMaximo = 0m, 
                CategoriasUtilizadas = 0, 
                GenerosUtilizados = 0 
            };
        }

        public async Task<bool> ActualizarEstadoAsync(int proId, bool activo)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE Productos SET ProActivo = @Activo, ProFechaActualizacion = GETDATE() WHERE ProId = @ProId",
                new { ProId = proId, Activo = activo }
            );

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Producto>> ListarConOfertasAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Producto>(@"
                SELECT DISTINCT p.* 
                FROM Productos p
                INNER JOIN ProductoTallaColor ptc ON p.ProId = ptc.ProId
                WHERE ptc.PrecioOferta IS NOT NULL 
                AND ptc.PrecioOferta > 0 
                AND p.ProActivo = 1
                ORDER BY p.ProFechaCreacion DESC");
        }

        public async Task<decimal> ObtenerPrecioPromedioAsync()
        {
            using var connection = _context.CreateConnection();
            var promedio = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(AVG(ProPrecio), 0) FROM Productos WHERE ProActivo = 1"
            );
            return promedio;
        }
    }
} 