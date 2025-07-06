using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Servicios
{
    public class CuponService : ICuponService
    {
        private readonly ICuponRepository _cuponRepository;

        public CuponService(ICuponRepository cuponRepository)
        {
            _cuponRepository = cuponRepository;
        }

        public Task<IEnumerable<Cupone>> ListarCuponesAsync()
            => _cuponRepository.ListarCuponesAsync();

        public Task<Cupone?> ObtenerPorIdAsync(int id)
            => _cuponRepository.ObtenerPorIdAsync(id);

        public Task<int> InsertarCuponAsync(Cupone cupon)
            => _cuponRepository.InsertarCuponAsync(cupon);

        public Task<bool> ActualizarCuponAsync(Cupone cupon)
            => _cuponRepository.ActualizarCuponAsync(cupon);

        public Task<bool> EliminarCuponAsync(int id)
            => _cuponRepository.EliminarCuponAsync(id);

        public Task<Cupone?> ObtenerPorCodigoAsync(string codigo)
            => _cuponRepository.ObtenerPorCodigoAsync(codigo);

        public Task<IEnumerable<Cupone>> ListarCuponesActivosAsync()
            => _cuponRepository.ListarCuponesActivosAsync();

        public Task<IEnumerable<Cupone>> ListarCuponesPorFechaAsync(DateTime fecha)
            => _cuponRepository.ListarCuponesPorFechaAsync(fecha);

        public Task<IEnumerable<Cupone>> ListarCuponesPorTipoAsync(string tipoDescuento)
            => _cuponRepository.ListarCuponesPorTipoAsync(tipoDescuento);

        public Task<bool> ValidarCuponAsync(string codigo, decimal montoCompra)
            => _cuponRepository.ValidarCuponAsync(codigo, montoCompra);

        public Task<bool> IncrementarUsoCuponAsync(int cuponId)
            => _cuponRepository.IncrementarUsoCuponAsync(cuponId);
    }
} 