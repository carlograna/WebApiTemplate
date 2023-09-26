using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiTemplate.Bd;
using WebApiTemplate.Modelo;
using WebApiTemplate.Request;
using WebApiTemplate.Validation;

namespace WebApiTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        List<Usuario> usu;
        private readonly PruebaContext _context;
        private readonly ILogger<UsuariosController> _logger;
        private readonly IConfiguration _config;

        public UsuariosController(PruebaContext context, IConfiguration config, ILogger<UsuariosController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

      
        //Login using Nlog
        //usin JWT for create Tokens
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
            _logger.LogWarning($"Intento de inicio de sesión fallido para el usuario {userLogin.Username}");
            return NotFound("Usuario no encontrado");
        }
        private Usuario Authenticate(LoginRequest userLogin)
        {
               
            var currentUser = _context.Usuario.ToList().FirstOrDefault(user => user.Username.ToLower() == userLogin.Username.ToLower()
                   && user.Password == userLogin.Password); ;
            if (currentUser != null)
            {
                Console.WriteLine("algo " + currentUser.Nombres);
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
            Console.WriteLine("claims " + claims[4]);
            // Crear el token
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            Console.WriteLine("token " + token);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // GET: api/Usuarios
        [HttpGet("listarU")]
        //[HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            if (_context.Usuario == null)
            {
                return NotFound();
            }
            return await _context.Usuario.ToListAsync();
        }

        

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            if (_context.Usuario == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("insertarU")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            var validador = new Validacio_Usuario();
            var resultado = validador.Validate(usuario);

            if (_context.Usuario == null)
            {
                return Problem("Entity set 'PruebaContext.Usuario'  is null.");
            }
            if (resultado.IsValid)
            {
                _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetUsuario", new { id = usuario.IdUsuario }, usuario);
        }

        //DELETE: api/Usuarios/5
        //[HttpDelete("{id}")]
        [HttpDelete("EliminarU{id}")]
        public async Task<dynamic> DeleteUsuario(int id)
        {
            var identify = HttpContext.User.Identity as ClaimsIdentity;
            var rtoken = Jwt.validarToken(identify, _context.Usuario.ToList());

            if (!rtoken.success)  return rtoken;
            Usuario usuario1 = rtoken.result;
            Console.WriteLine("Yes" + usuario1.ToString);
            if (usuario1.Rol != "Administrador")
            {
                return new
                {
                    Success = false,
                    Message = "Revise que su token sea válido",
                    result = ""
                };
            }
            else
            {
                if (_context.Usuario == null)
                {
                    return NotFound();
                }
                var usuario = await _context.Usuario.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                _context.Usuario.Remove(usuario);
                await _context.SaveChangesAsync();

                //return NoContent();
            }


                return _context.Usuario;
        }


        
        private bool UsuarioExists(int id)
        {
            return (_context.Usuario?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }

        
    }
}
