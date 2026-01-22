using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationProject.Models
{
    public class Roslina
    {
        public Guid Id { get; set; }
        public string Nazwa { get; set; } = null!;
        public Guid LokacjaId { get; set; }
        [ForeignKey(nameof(LokacjaId))]
        public Lokacja? Lokacja { get; set; }
    }
}
