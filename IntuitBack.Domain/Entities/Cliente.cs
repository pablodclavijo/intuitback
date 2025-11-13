namespace IntuitBack.Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombres { get; set; } = "";
        public string Apellidos { get; set; } = "";

        public DateTime FechaNacimiento { get; set; }

        public string Cuit { get; set; } = "";

        public string Domicilio { get; set; } = "";

        public string TelefonoCelular { get; set; } = "";

        public string Email { get; set; } = "";

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool Eliminado { get; set; } = false;
    }
}
