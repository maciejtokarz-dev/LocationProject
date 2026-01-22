using System.ComponentModel.DataAnnotations;          // Do [Required] itp.
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationProject.Models
{
    public class Zwierze
    {
        public Guid Id { get; set; }
        public string Nazwa { get; set; } = null!;   
        public Guid LokacjaId{ get; set; }
        [ForeignKey(nameof(LokacjaId))]
        public Lokacja? Lokacja { get; set; }   
    }
}
