
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Data;
using WebApiTemplate.Database;
using WebApiTemplate.Request;
using WebApiTemplate.Validation;

namespace WebApiTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly UserContext _context;
        //private readonly ILogger<UserController> _logger;
        // private readonly IConfiguration _config;
        // private User1 user1;
        private readonly UserRepository _employeeRepository;
        TokenManager _tokenManager;

        //public UserController(UserContext context, IConfiguration config, ILogger<UserController> logger, UserRepository employeeRepository)
        public UserController(UserRepository employeeRepository)
        {
            //_context = context;
            //_config = config;
            //_logger = logger;
            _tokenManager = new TokenManager();
            _employeeRepository = employeeRepository;
        }


        // GET: api/Usuarios
        [HttpGet("GetUser")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User1>>> GetUser()
        //public async Task<ActionResult<IEnumerable<String>>> GetUser()
        // public async Task<ActionResult> GetUser()
        {
            string token = _tokenManager.GetToken();
            Console.WriteLine("token generate "+token);
            var employeeList = await _employeeRepository.GetAllEmployeeAsync();
            return Ok(employeeList);
        }



        //// GET: api/Usuarios/5
        [HttpGet("GetUser{id}")]
        [Authorize]
        public async Task<ActionResult<User1>> GetUser([FromRoute] int id)
        {
            var user = await _employeeRepository.GetEmployeeByIdAsync(id);
            return user;
        }

        ////// PUT: api/Usuarios/5
        [HttpPut("UpdateUser{id}")]
        [Authorize]
        public async Task<dynamic> PutUser([FromRoute] int id, [FromBody] User1 model)
        {
            await _employeeRepository.UpdateEmployeeAsync(id, model);
            return Ok("User updated");

        }

        // POST: api/Usuarios
        [HttpPost("insertuser")]
        [Authorize]
        public async Task<ActionResult> AddUser([FromBody] User1 model)
        {
               
            await _employeeRepository.AddEmployeeAsync(model);
            return Ok();
        }

        ////    //DELETE: api/Usuarios/5
        [HttpDelete("DeleteUser{id}")]
        //[Authorize]
        public async Task<dynamic> DeleteUsuario(int id)
        {
            await _employeeRepository.DeleteEmployeeAsnyc(id);
            return Ok();
        }
        //private bool UsuarioExists(int id)
        //{
        //    return (_context.User1?.Any(e => e.IdUser == id)).GetValueOrDefault();
        //}

        //public static class DataStorage
        //{
        //    public static string StoredData { get; set; }
        //}

    }
    
}
