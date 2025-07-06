using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public Task<IEnumerable<Categoria>> ListarCategoriasAsync()
            => _categoriaRepository.ListarCategoriasAsync();

        public Task<Categoria?> ObtenerPorIdAsync(int id)
            => _categoriaRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarCategoriaAsync(Categoria categoria)
            => _categoriaRepository.InsertarCategoriaAsync(categoria);

        public Task<bool> ActualizarCategoriaAsync(Categoria categoria)
            => _categoriaRepository.ActualizarCategoriaAsync(categoria);

        public Task<bool> EliminarCategoriaAsync(int id)
            => _categoriaRepository.EliminarCategoriaAsync(id);

        public Task<IEnumerable<Categoria>> ListarCategoriasActivasAsync()
            => _categoriaRepository.ListarCategoriasActivasAsync();

        public Task<IEnumerable<Categoria>> ListarCategoriasPadreAsync()
            => _categoriaRepository.ListarCategoriasPadreAsync();

        public Task<IEnumerable<Categoria>> ListarSubcategoriasAsync(int categoriaPadreId)
            => _categoriaRepository.ListarSubcategoriasAsync(categoriaPadreId);

        public Task<Categoria?> ObtenerPorNombreAsync(string nombre)
            => _categoriaRepository.ObtenerPorNombreAsync(nombre);

        public Task<IEnumerable<Categoria>> ListarCategoriasConProductosAsync()
            => _categoriaRepository.ListarCategoriasConProductosAsync();
    }
} 