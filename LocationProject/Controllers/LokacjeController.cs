using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocationProject.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using LocationProject.Models;
using Microsoft.AspNetCore.Authorization;
using LocationProject.DTOs;

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
        public async Task<IActionResult> GetLokacje(
            [FromQuery] string? name,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20
            )
        {
            if(pageSize > 100)
            {
                pageSize = 100;
            }

            var query = _context.Lokacje.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(l => l.Nazwa.Contains(name));
            }
            var dtoQuery = query
                 .Select(l => new LokacjaDto
                 {
                     Id = l.Id,
                     Nazwa = l.Nazwa,
                     Opis = l.Opis,
                     LiczbaRoslin = l.Rosliny.Count(),
                     LiczbaZwierzatek = l.Zwierzeta.Count()
                 });
            dtoQuery = (sortBy, sortDirection) switch
            {
                ("nazwa", "desc") => dtoQuery.OrderByDescending(x => x.Nazwa),
                ("nazwa", _) => dtoQuery.OrderBy(l => l.Nazwa),
                ("LiczbaRoslin", "desc") => dtoQuery.OrderByDescending(x => x.LiczbaRoslin),
                ("LiczbaRoslin", _) => dtoQuery.OrderBy(l => l.LiczbaRoslin),
                ("LiczbaZwierzatek", "desc") => dtoQuery.OrderByDescending(x => x.LiczbaZwierzatek),
                ("LiczbaZwierzatek", _) => dtoQuery.OrderBy(l => l.LiczbaZwierzatek),
                _ => dtoQuery.OrderBy(x => x.Nazwa)
            };
            var totalCount = await query.CountAsync();

            dtoQuery = dtoQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var result = await dtoQuery.ToListAsync();

            return Ok(new
            {
                Items = result,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }
        [HttpGet("{id}/podsumowanie")]
        public async Task<IActionResult> GetLokacjaPodsumowanie(Guid id)
        {
            var getPodsumowanie = await _context.Lokacje
                    .Where(l => l.Id == id)
                    .Select(l => new LokacjaDto
                    {
                        Id = l.Id,
                        Nazwa = l.Nazwa,
                        Opis = l.Opis,
                        LiczbaRoslin = l.Rosliny.Count(),
                        LiczbaZwierzatek = l.Zwierzeta.Count()
                    })
                    .FirstOrDefaultAsync();

            if (getPodsumowanie == null)
            {
                return NotFound();
            }

            return Ok(getPodsumowanie);
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
