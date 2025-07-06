using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BD.Models;

namespace Ecommerce.PRC.Interfaces
{
    public interface ILogService
    {
        Task<IEnumerable<Log>> ListarLogsAsync();
        Task<Log?> ObtenerPorIdAsync(int id);
        Task<int> InsertarLogAsync(Log log);
        Task<bool> EliminarLogAsync(int id);
    }
}