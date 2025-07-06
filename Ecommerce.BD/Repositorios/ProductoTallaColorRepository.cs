using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class ProductoTallaColorRepository : IProductoTallaColorRepository
    {
        private readonly DapperContext _context;

        public ProductoTallaColorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoTallaColor>("ProductoTallaColor_List");
        }

        public async Task<ProductoTallaColor?> ObtenerPorIdAsync(int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            var productos = await connection.QueryAsync<ProductoTallaColor>(
                "SELECT * FROM ProductoTallaColor WHERE ProductoTallaColorId = @ProductoTallaColorId",
                new { ProductoTallaColorId = productoTallaColorId }
            );
            return productos.FirstOrDefault();
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarPorProductoAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoTallaColor>(
                "ProductoTallaColor_ListPorProducto",
                new { ProId = proId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarPorTallaAsync(int tallaId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoTallaColor>(
                "SELECT * FROM ProductoTallaColor WHERE TallaId = @TallaId ORDER BY FechaCreacion DESC",
                new { TallaId = tallaId }
            );
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarPorColorAsync(int colorId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoTallaColor>(
                "SELECT * FROM ProductoTallaColor WHERE ColorId = @ColorId ORDER BY FechaCreacion DESC",
                new { ColorId = colorId }
            );
        }

        public async Task<int> CrearAsync(ProductoTallaColor productoTallaColor)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProId", productoTallaColor.ProId);
            parameters.Add("@TallaId", productoTallaColor.TallaId);
            parameters.Add("@ColorId", productoTallaColor.ColorId);
            parameters.Add("@Stock", productoTallaColor.Stock);
            parameters.Add("@PrecioOferta", productoTallaColor.PrecioOferta);
            parameters.Add("@SKU", productoTallaColor.Sku);
            parameters.Add("@ProductoTallaColorId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "ProductoTallaColor_Insert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ProductoTallaColorId");
        }

        public async Task<bool> ActualizarAsync(ProductoTallaColor productoTallaColor)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductoTallaColorId", productoTallaColor.ProductoTallaColorId);
            parameters.Add("@Stock", productoTallaColor.Stock);
            parameters.Add("@PrecioOferta", productoTallaColor.PrecioOferta);
            parameters.Add("@SKU", productoTallaColor.Sku);

            var rowsAffected = await connection.ExecuteAsync(
                "ProductoTallaColor_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> EliminarAsync(int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "ProductoTallaColor_Delete",
                new { ProductoTallaColorId = productoTallaColorId },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ExisteAsync(int productoTallaColorId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM ProductoTallaColor WHERE ProductoTallaColorId = @ProductoTallaColorId",
                new { ProductoTallaColorId = productoTallaColorId }
            );
            return count > 0;
        }

        public async Task<bool> ExisteCombinacionAsync(int proId, int tallaId, int colorId)
        {
            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM ProductoTallaColor WHERE ProId = @ProId AND TallaId = @TallaId AND ColorId = @ColorId",
                new { ProId = proId, TallaId = tallaId, ColorId = colorId }
            );
            return count > 0;
        }

        public async Task<ProductoTallaColor?> ObtenerCombinacionAsync(int proId, int tallaId, int colorId)
        {
            using var connection = _context.CreateConnection();
            var productos = await connection.QueryAsync<ProductoTallaColor>(
                "SELECT * FROM ProductoTallaColor WHERE ProId = @ProId AND TallaId = @TallaId AND ColorId = @ColorId",
                new { ProId = proId, TallaId = tallaId, ColorId = colorId }
            );
            return productos.FirstOrDefault();
        }

        public async Task<bool> ActualizarStockAsync(int productoTallaColorId, int nuevoStock)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE ProductoTallaColor SET Stock = @NuevoStock, FechaActualizacion = GETDATE() WHERE ProductoTallaColorId = @ProductoTallaColorId",
                new { ProductoTallaColorId = productoTallaColorId, NuevoStock = nuevoStock }
            );

            return rowsAffected > 0;
        }

        public async Task<bool> ActualizarPrecioOfertaAsync(int productoTallaColorId, decimal? precioOferta)
        {
            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE ProductoTallaColor SET PrecioOferta = @PrecioOferta, FechaActualizacion = GETDATE() WHERE ProductoTallaColorId = @ProductoTallaColorId",
                new { ProductoTallaColorId = productoTallaColorId, PrecioOferta = precioOferta }
            );

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarConStockAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoTallaColor>(
                "SELECT * FROM ProductoTallaColor WHERE Stock > 0 ORDER BY FechaCreacion DESC"
            );
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarSinStockAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoTallaColor>(
                "SELECT * FROM ProductoTallaColor WHERE Stock = 0 ORDER BY FechaCreacion DESC"
            );
        }

        public async Task<IEnumerable<ProductoTallaColor>> ListarConOfertasAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ProductoTallaColor>(
                "SELECT * FROM ProductoTallaColor WHERE PrecioOferta IS NOT NULL AND PrecioOferta > 0 ORDER BY FechaCreacion DESC"
            );
        }

        public async Task<object> ObtenerEstadisticasStockAsync()
        {
            using var connection = _context.CreateConnection();
            
            var estadisticas = await connection.QueryFirstOrDefaultAsync(@"
                SELECT 
                    COUNT(*) AS TotalCombinaciones,
                    COUNT(CASE WHEN Stock > 0 THEN 1 END) AS ConStock,
                    COUNT(CASE WHEN Stock = 0 THEN 1 END) AS SinStock,
                    ISNULL(SUM(Stock), 0) AS StockTotal,
                    ISNULL(AVG(Stock), 0) AS StockPromedio,
                    COUNT(CASE WHEN PrecioOferta IS NOT NULL AND PrecioOferta > 0 THEN 1 END) AS ConOfertas,
                    COUNT(DISTINCT ProId) AS ProductosConCombinaciones,
                    COUNT(DISTINCT TallaId) AS TallasUtilizadas,
                    COUNT(DISTINCT ColorId) AS ColoresUtilizados
                FROM ProductoTallaColor");

            return estadisticas ?? new { 
                TotalCombinaciones = 0, 
                ConStock = 0, 
                SinStock = 0, 
                StockTotal = 0, 
                StockPromedio = 0m, 
                ConOfertas = 0, 
                ProductosConCombinaciones = 0, 
                TallasUtilizadas = 0, 
                ColoresUtilizados = 0 
            };
        }

        public async Task<bool> VerificarStockDisponibleAsync(int productoTallaColorId, int cantidad)
        {
            using var connection = _context.CreateConnection();
            var stock = await connection.ExecuteScalarAsync<int>(
                "SELECT Stock FROM ProductoTallaColor WHERE ProductoTallaColorId = @ProductoTallaColorId",
                new { ProductoTallaColorId = productoTallaColorId }
            );
            return stock >= cantidad;
        }

        public async Task<decimal> ObtenerPrecioMinimoAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var precioMinimo = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(MIN(PrecioOferta), 0) FROM ProductoTallaColor WHERE ProId = @ProId AND PrecioOferta IS NOT NULL AND PrecioOferta > 0",
                new { ProId = proId }
            );
            return precioMinimo;
        }

        public async Task<decimal> ObtenerPrecioMaximoAsync(int proId)
        {
            using var connection = _context.CreateConnection();
            var precioMaximo = await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(MAX(PrecioOferta), 0) FROM ProductoTallaColor WHERE ProId = @ProId AND PrecioOferta IS NOT NULL AND PrecioOferta > 0",
                new { ProId = proId }
            );
            return precioMaximo;
        }
    }
} 