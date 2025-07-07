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

        public async Task<Talla?> ObtenerPorIdAsync(int id)
        {
            try
            {
                Console.WriteLine($"TallaService: Intentando obtener talla con ID {id}");
                var result = await _tallaRepository.ObtenerPorIdAsync(id);
                Console.WriteLine($"TallaService: Resultado obtenido: {result?.TallaId ?? -1}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TallaService: Error al obtener talla con ID {id}: {ex.Message}");
                throw;
            }
        }

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