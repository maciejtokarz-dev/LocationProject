using LocationProject.Models;

namespace LocationProject.DTOs
{
    public class LokacjaDto
    {
        public Guid Id { get; set; }
        public string Nazwa { get; set; } = null!;
        public string? Opis { get; set; }
        public int LiczbaRoslin { get; set; } 
        public int LiczbaZwierzatek { get; set; } 
    }
}
