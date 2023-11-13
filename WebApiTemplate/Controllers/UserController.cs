using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using WebApiTemplate.Database;
using WebApiTemplate.Model;
using WebApiTemplate.Request;
using WebApiTemplate.Services;
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
            
            var userService = new UserService();
            var user = await userService.LoginJwt(userLogin, _context);
           if (user != null)
            {
                // Crear el token
                var token = userService.Generate(user, _config);
                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            
            return NotFound("User not found");
        }
       

        // GET: api/Usuarios
        [HttpGet("GetUser")]
        public async Task<ActionResult<IEnumerable<User1>>> GetUser()
        {
            if (_context.User1 == null)
            {
                return NotFound();
            }
            return await _context.User1.ToListAsync();
        }



        // GET: api/Usuarios/5
        [HttpGet("GetUser{id}")]
        public async Task<ActionResult<User1>> GetUser(int id)
        {
            if (_context.User1 == null)
            {
                return NotFound();
            }
            var user = await _context.User1.FindAsync(id);

            if (user == null)
            {
                return NotFound("The user wasn't found");
            }

            return user;
        }

        // PUT: api/Usuarios/5
        [HttpPut("UpdateUser{id}")]
        public async Task<IActionResult> PutUser(int id, User1 user)
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
        public async Task<ActionResult<User1>> PostUsuario(User1 user)
        {
            var validador = new UserValidation();
            var resultadoName = validador.NotEmptyNames(user.Names);
            var resultadoPassword = validador.IsValidPassword(user.Password);
            var resultadoEmail = validador.IsValidEmail(user.Email);

            var registration_date = DateTime.Today;
            //var date= "registration_date.Month."


            if (_context.User1 == null)
            {
                return Problem("Entity set 'UserContext.User is null.");
            }
            if (resultadoName)
            {
                if (resultadoPassword)
                {
                    if (resultadoEmail)
                    {
                        //user.Registration_date = registration_date;
                        _context.User1.Add(user);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound("The email must be valid");
                    }
                }
                else
                {
                    return NotFound("Password can't be empty");
                }


            }
            else
            {
                return NotFound("Name can't be empty ");
            }
            return CreatedAtAction("GetUser", new { id = user.IdUser }, user);
        }

        //DELETE: api/Usuarios/5
        [HttpDelete("DeleteUser{id}")]
        public async Task<dynamic> DeleteUsuario(int id)
        {
            var identify = HttpContext.User.Identity as ClaimsIdentity;
            var rtoken = Jwt.validateToken(identify, _context.User1.ToList());

            if (!rtoken.success) return rtoken;
            User1 usuario1 = rtoken.result;
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
                if (_context.User1 == null)
                {
                    return NotFound("The user wasn't found");
                }
                var usuario = await _context.User1.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound("The user wasn't found");
                }

                _context.User1.Remove(usuario);
                await _context.SaveChangesAsync();

                return Ok(usuario);
            }
        }
        private bool UsuarioExists(int id)
        {
            return (_context.User1?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }


    }
}
