using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class PromocionService : IPromocionService
    {
        private readonly IPromocionRepository _promocionRepository;

        public PromocionService(IPromocionRepository promocionRepository)
        {
            _promocionRepository = promocionRepository;
        }

        public Task<IEnumerable<Promocione>> ListarPromocionesAsync()
            => _promocionRepository.ListarPromocionesAsync();

        public Task<Promocione?> ObtenerPorIdAsync(int id)
            => _promocionRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarPromocionAsync(Promocione promocion)
            => _promocionRepository.InsertarPromocionAsync(promocion);

        public Task<bool> ActualizarPromocionAsync(Promocione promocion)
            => _promocionRepository.ActualizarPromocionAsync(promocion);

        public Task<bool> EliminarPromocionAsync(int id)
            => _promocionRepository.EliminarPromocionAsync(id);

        public Task<IEnumerable<Promocione>> ListarPromocionesActivasAsync()
            => _promocionRepository.ListarPromocionesActivasAsync();

        public Task<IEnumerable<Promocione>> ListarPromocionesPorFechaAsync(DateTime fecha)
            => _promocionRepository.ListarPromocionesPorFechaAsync(fecha);

        public Task<IEnumerable<Promocione>> ListarPromocionesPorTipoAsync(string tipoDescuento)
            => _promocionRepository.ListarPromocionesPorTipoAsync(tipoDescuento);
    }
} 