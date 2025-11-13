
namespace IntuitBack.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }

        public string Nivel { get; set; } = "Info";

        public string Mensaje { get; set; } = "";

        public string? Detalle { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
