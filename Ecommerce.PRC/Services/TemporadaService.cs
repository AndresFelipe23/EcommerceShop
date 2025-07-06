using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class TemporadaService : ITemporadaService
    {
        private readonly ITemporadaRepository _temporadaRepository;

        public TemporadaService(ITemporadaRepository temporadaRepository)
        {
            _temporadaRepository = temporadaRepository;
        }

        public Task<IEnumerable<Temporada>> ListarTemporadasAsync()
            => _temporadaRepository.ListarTemporadasAsync();

        public Task<Temporada?> ObtenerPorIdAsync(int id)
            => _temporadaRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarTemporadaAsync(Temporada temporada)
            => _temporadaRepository.InsertarTemporadaAsync(temporada);

        public Task<bool> ActualizarTemporadaAsync(Temporada temporada)
            => _temporadaRepository.ActualizarTemporadaAsync(temporada);

        public Task<bool> EliminarTemporadaAsync(int id)
            => _temporadaRepository.EliminarTemporadaAsync(id);

        public Task<IEnumerable<Temporada>> ListarTemporadasActivasAsync()
            => _temporadaRepository.ListarTemporadasActivasAsync();

        public Task<IEnumerable<Temporada>> ListarTemporadasPorFechaAsync(DateTime fecha)
            => _temporadaRepository.ListarTemporadasPorFechaAsync(fecha);
    }
} 