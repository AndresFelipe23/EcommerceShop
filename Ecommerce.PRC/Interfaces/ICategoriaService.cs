using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ListarCategoriasAsync();
        Task<Categoria?> ObtenerPorIdAsync(int id);
        Task<int> InsertarCategoriaAsync(Categoria categoria);
        Task<bool> ActualizarCategoriaAsync(Categoria categoria);
        Task<bool> EliminarCategoriaAsync(int id);
        Task<IEnumerable<Categoria>> ListarCategoriasActivasAsync();
        Task<IEnumerable<Categoria>> ListarCategoriasPadreAsync();
        Task<IEnumerable<Categoria>> ListarSubcategoriasAsync(int categoriaPadreId);
        Task<Categoria?> ObtenerPorNombreAsync(string nombre);
        Task<IEnumerable<Categoria>> ListarCategoriasConProductosAsync();
    }
} 