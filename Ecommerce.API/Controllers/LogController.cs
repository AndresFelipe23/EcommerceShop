using Ecommerce.BD.Models;
using Ecommerce.PRC.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        // GET: api/Log
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _logService.ListarLogsAsync();
            return Ok(logs);
        }

        // GET: api/Log/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLog(int id)
        {
            var log = await _logService.ObtenerPorIdAsync(id);
            if (log == null) return NotFound();

            return Ok(log);
        }

        // POST: api/Log
        [HttpPost]
        public async Task<IActionResult> PostLog([FromBody] Log log)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _logService.InsertarLogAsync(log);
            return CreatedAtAction(nameof(GetLog), new { id }, log);
        }

        // DELETE: api/Log/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var result = await _logService.EliminarLogAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
