using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocationProject.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using LocationProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace LocationProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/lokacje")]
    public class lokacjeController : ControllerBase
    {
        private readonly LokacjeContext _context;

        public lokacjeController(LokacjeContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetLokacje()
        {
            var lokacje = await _context.Lokacje.ToListAsync();
            return Ok(lokacje);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLokacja(Guid id)
        {
            var lokacja = await _context.Lokacje.FindAsync(id);
            if (lokacja == null)
            {
                return NotFound();
            }
            return Ok(lokacja);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchLokacje(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Ok(new List<Lokacja>());
            }

            var lokacje = await _context.Lokacje
                .Where(l => l.Opis != null && l.Opis.Contains(query) || l.Nazwa.Contains(query))
                .ToListAsync();

            return Ok(lokacje);
            
        }
        [HttpPost]
        public async Task<IActionResult> CreateLokacja([FromBody] Lokacja nowaLokacja)
        {
            nowaLokacja.Id = Guid.NewGuid();
            _context.Lokacje.Add(nowaLokacja);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLokacja), new { id = nowaLokacja.Id }, nowaLokacja);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLokacja(Guid Id, [FromBody] Lokacja zaktualizowanaLokacja)
        {
            if(Id != zaktualizowanaLokacja.Id)
            {
                return BadRequest();
            }

            var istniejacaLokacja = await _context.Lokacje.FindAsync(Id);
            
            if(istniejacaLokacja == null)
            {
                return NotFound();
            }
            istniejacaLokacja.Nazwa = zaktualizowanaLokacja.Nazwa;
            istniejacaLokacja.Opis = zaktualizowanaLokacja.Opis;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLokacja(Guid Id)
        {
            var lokacjaDoUsuniecia = await _context.Lokacje.FindAsync(Id);
            if (lokacjaDoUsuniecia == null)
            {
                return NotFound();
            }
            _context.Lokacje.Remove(lokacjaDoUsuniecia);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
