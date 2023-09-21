using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiTemplate.Data;
using WebApiTemplate.Modelo;
using WebApiTemplate.Request;

namespace WebApiTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _config;

        public UsuarioController(IConfiguration config)
        {
            _config = config;
            
        }

        [HttpPost("loginJWT")]
        public IActionResult LoginJwt(LoginRequest userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                // Crear el token
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("Usuario no encontrado");
        }
        private Usuario Authenticate(LoginRequest userLogin)
        {
            var currentUser = UsuarioData.Listar().FirstOrDefault(user => user.Username.ToLower() == userLogin.Username.ToLower()
                   && user.Password == userLogin.Password); ;
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
        private string Generate(Usuario user)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Crear los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim(ClaimTypes.GivenName, user.Nombres),
                new Claim(ClaimTypes.Surname, user.Telefono),
                new Claim(ClaimTypes.Role, user.Rol),
                new Claim("id", user.IdUsuario.ToString()),

            };
            // Crear el token
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // GET api/<controller>
        [HttpGet("listarU")]
        public List<Usuario> Get()
        {
            return UsuarioData.Listar();
        }

        // GET api/<controller>/5
        [HttpGet("listarU{id}")]
        public Usuario Get(int id)
        {
            return UsuarioData.Obtener(id);
        }

        // POST api/<controller>
        [HttpPost("insertarU")]
        public bool Post([FromBody] Usuario oUsuario)
        {
            return UsuarioData.Registrar(oUsuario);
        }  

        // PUT api/<controller>/5
        [HttpPut("ActualizarU{id}")]
        public bool Put([FromBody] Usuario oUsuario)
        {
            return UsuarioData.Modificar(oUsuario);
        }

        // DELETE api/<controller>/5
        [HttpDelete("EliminarU{id}")]
        public dynamic Delete(int id)
        {
            var identify = HttpContext.User.Identity as ClaimsIdentity;
            var rtoken = Jwt.validarToken(identify);
            if (!rtoken.success) return rtoken;
            Usuario usuario = rtoken.result;
            if (usuario.Rol != "Administrador")
            {
                return new
                {
                    Success = false,
                    Message = "Revise que su token sea válido",
                    result = ""
                };
            }
            return UsuarioData.Eliminar(id);
        }
    }
}
