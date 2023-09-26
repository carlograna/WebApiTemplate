using System.Security.Claims;
using WebApiTemplate.Bd;

namespace WebApiTemplate.Modelo
{
    public class Jwt
    {
        public string key { get; set; }
        public string Issuer { get; set; }
        public string  Audience { get; set; }

        public string Subject { get; set; }


        public static dynamic validarToken(ClaimsIdentity identify, List<Usuario> usuar)
        {
            try
            {
                if (identify.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "verificar si estas enviando un token válido",
                        result = ""
                    };
                }


                var id = identify.Claims.FirstOrDefault(x => x.Type == "id").Value;
                Usuario usuario = usuar.FirstOrDefault(x => x.IdUsuario == Int32.Parse(id)) ?? null;

                return new
                {
                    success = true,
                    message = "exito",
                    result = usuario
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }
    }
}
