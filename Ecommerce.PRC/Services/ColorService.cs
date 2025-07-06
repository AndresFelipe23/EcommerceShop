using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public Task<IEnumerable<Colore>> ListarColoresAsync()
            => _colorRepository.ListarColoresAsync();

        public Task<Colore?> ObtenerPorIdAsync(int id)
            => _colorRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarColorAsync(Colore color)
            => _colorRepository.InsertarColorAsync(color);

        public Task<bool> ActualizarColorAsync(Colore color)
            => _colorRepository.ActualizarColorAsync(color);

        public Task<bool> EliminarColorAsync(int id)
            => _colorRepository.EliminarColorAsync(id);

        public Task<Colore?> ObtenerPorNombreAsync(string nombre)
            => _colorRepository.ObtenerPorNombreAsync(nombre);

        public Task<Colore?> ObtenerPorCodigoHexAsync(string codigoHex)
            => _colorRepository.ObtenerPorCodigoHexAsync(codigoHex);
    }
} 