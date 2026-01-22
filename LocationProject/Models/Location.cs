using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LocationProject.Models
{
    public class Lokacja
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Nazwa { get; set; } = null!;
        [MaxLength(1000)]
        public string? Opis { get; set; }
        public ICollection<Roslina>? Rosliny { get; set; } = new List<Roslina>();
        public ICollection<Zwierze>? Zwierzeta { get; set; } = new List<Zwierze>();

    }
}
