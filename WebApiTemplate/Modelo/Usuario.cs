namespace WebApiTemplate.Modelo
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string Nombres { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
