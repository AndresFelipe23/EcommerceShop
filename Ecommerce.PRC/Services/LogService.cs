using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;

namespace Ecommerce.PRC.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public Task<IEnumerable<Log>> ListarLogsAsync()
        {
            return _logRepository.ListarLogsAsync();
        }

        public Task<Log?> ObtenerPorIdAsync(int id)
        {
            return _logRepository.ObtenerPorIdAsync(id);
        }

        public Task<int> InsertarLogAsync(Log log)
        {
            return _logRepository.InsertarLogAsync(log);
        }

        public Task<bool> EliminarLogAsync(int id)
        {
            return _logRepository.EliminarLogAsync(id);
        }
    }
}
