using Dapper;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using System.Data;

namespace Ecommerce.BD.Repositorios
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly DapperContext _context;

        public CategoriaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ListarCategoriasAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Categoria>("Categoria_List", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<Categoria?> ObtenerPorIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Categoria>(
                "Categoria_ListPorId",
                new { CatId = id },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<int> InsertarCategoriaAsync(Categoria categoria)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "Categoria_Insert",
                new { 
                    categoria.CatNombre, 
                    categoria.CatDescripcion, 
                    categoria.CatPadreId, 
                    categoria.CatOrden, 
                    categoria.CatActivo,
                    CatFechaCreacion = DateTime.Now
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<bool> ActualizarCategoriaAsync(Categoria categoria)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Categoria_Update",
                new { 
                    categoria.CatId, 
                    categoria.CatNombre, 
                    categoria.CatDescripcion, 
                    categoria.CatPadreId, 
                    categoria.CatOrden, 
                    categoria.CatActivo 
                },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "Categoria_Delete",
                new { CatId = id },
                commandType: CommandType.StoredProcedure
            );
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Categoria>> ListarCategoriasActivasAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Categoria>(
                "Categoria_ListActivas",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Categoria>> ListarCategoriasPadreAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Categoria>(
                "Categoria_ListPadres",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Categoria>> ListarSubcategoriasAsync(int categoriaPadreId)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Categoria>(
                "Categoria_ListSubcategorias",
                new { CatPadreId = categoriaPadreId },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<Categoria?> ObtenerPorNombreAsync(string nombre)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Categoria>(
                "Categoria_ListPorNombre",
                new { CatNombre = nombre },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }

        public async Task<IEnumerable<Categoria>> ListarCategoriasConProductosAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<Categoria>(
                "Categoria_ListConProductos",
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
} 