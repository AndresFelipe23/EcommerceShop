using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class TallaService : ITallaService
    {
        private readonly ITallaRepository _tallaRepository;

        public TallaService(ITallaRepository tallaRepository)
        {
            _tallaRepository = tallaRepository;
        }

        public Task<IEnumerable<Talla>> ListarTallasAsync()
            => _tallaRepository.ListarTallasAsync();

        public Task<Talla?> ObtenerPorIdAsync(int id)
            => _tallaRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarTallaAsync(Talla talla)
            => _tallaRepository.InsertarTallaAsync(talla);

        public Task<bool> ActualizarTallaAsync(Talla talla)
            => _tallaRepository.ActualizarTallaAsync(talla);

        public Task<bool> EliminarTallaAsync(int id)
            => _tallaRepository.EliminarTallaAsync(id);

        public Task<IEnumerable<Talla>> ListarTallasPorGeneroAsync(string genero)
            => _tallaRepository.ListarTallasPorGeneroAsync(genero);
    }
} 