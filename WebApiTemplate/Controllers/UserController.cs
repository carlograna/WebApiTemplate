using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiTemplate.Database;
using WebApiTemplate.Model;
using WebApiTemplate.Request;
using WebApiTemplate.Validation;

namespace WebApiTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;

        public UserController(UserContext context, IConfiguration config, ILogger<UserController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

      
        //Login using Nlog
        //usin JWT for create Tokens
        [HttpPost("loginJWT")]
        public async Task<IActionResult> LoginJwt(LoginRequest userLogin)
        {
            var user = await Authenticate(userLogin);
            if (user != null)
            {
                // Crear el token
                var token = Generate(user);
                return Ok(token);
            }
            _logger.LogWarning($"Failed login attempt for user: {userLogin.Username}");
            return NotFound("User not found");
        }
        private async Task<User> Authenticate(LoginRequest userLogin)
        {
            await Task.Delay(2000);
            var currentUser = _context.User?.ToList().FirstOrDefault(user => user.Username.ToLower() == userLogin.Username.ToLower()
                   && user.Password == userLogin.Password); ;
            if (currentUser != null)
            {
               return currentUser;
            }
            return null;
        }
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Crear los claims
            var claims = new[]
            {
                        new Claim(ClaimTypes.NameIdentifier, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.GivenName, user.Names),
                        new Claim(ClaimTypes.Surname, user.Phone),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("id", user.IdUser.ToString()),
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

        // GET: api/Usuarios
        [HttpGet("GetUser")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            return await _context.User.ToListAsync();
        }

        

        // GET: api/Usuarios/5
        [HttpGet("GetUser{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound("The user wasn't found");
            }

            return user;
        }

        // PUT: api/Usuarios/5
        [HttpPut("UpdateUser{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.IdUser)
            {
                return BadRequest("There is no user with that id");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound("The user doesn't exist");
                }
                else
                {
                    throw;
                }
            }
            return Ok("The user was successfully modified");
        }

        // POST: api/Usuarios
        [HttpPost("insertUser")]
        public async Task<ActionResult<User>> PostUsuario(User user)
        {
            var validador = new UserValidation();
            var resultado = validador.Validate(user);

            if (_context.User == null)
            {
                return Problem("Entity set 'UserContext.User is null.");
            }
            if (resultado.IsValid)
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetUser", new { id = user.IdUser }, user);
        }

        //DELETE: api/Usuarios/5
        [HttpDelete("DeleteUser{id}")]
        public async Task<dynamic> DeleteUsuario(int id)
        {
            var identify = HttpContext.User.Identity as ClaimsIdentity;
            var rtoken = Jwt.validateToken(identify, _context.User.ToList());

            if (!rtoken.success)  return rtoken;
            User usuario1 = rtoken.result;
            Console.WriteLine("Yes" + usuario1.ToString);
            if (usuario1.Role != "Administrador")
            {
                return new
                {
                    Success = false,
                    Message = "Check that your token is valid",
                    result = ""
                };
            }
            else
            {
                if (_context.User == null)
                {
                    return NotFound("The user wasn't found");
                }
                var usuario = await _context.User.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound("The user wasn't found");
                }

                _context.User.Remove(usuario);
                await _context.SaveChangesAsync();

                return Ok(usuario);
            }   
        }        
        private bool UsuarioExists(int id)
        {
            return (_context.User?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }

        
    }
}
