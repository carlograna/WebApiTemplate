
using DotNetOpenAuth.AspNet.Clients;
using Facebook;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebApiTemplate.Data;
using WebApiTemplate.Model;
using WebApiTemplate.Request;

namespace WebApiTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _employeeRepository;
        private readonly JwtOption _options;
        private readonly TokenManager _tokenManager;

        public AuthController(UserRepository employeeRepository, IOptions<JwtOption> options, IConfiguration configuration)
        {
            _employeeRepository = employeeRepository;
            _options = options.Value;
            _tokenManager = new TokenManager();
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest model)
        {
            var employee = await _employeeRepository.GetEmplaoyeeByUsernamel(model.Username);
            if (employee is null)
            {
                return BadRequest(new { error = "email does not exist" });
            }
            if (employee.Password != model.Password)
            {
                return BadRequest(new { error = "email/password is incorrect." });
            }
            var token = GetJWTToken(model.Username);
            _tokenManager.SetToken(token);
            return Ok(new { response = token });
        }

        private string GetJWTToken(string email)
        {
            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var crendential = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>()
            {
                new Claim("Email",email)
            };
            var sToken = new JwtSecurityToken(_options.Key, _options.Issuer, claims, expires: DateTime.Now.AddDays(7), signingCredentials: crendential);
            var token = new JwtSecurityTokenHandler().WriteToken(sToken);
            return token;
        }
        [HttpPost("token")]
        public async Task<ActionResult> GoogleLogin([FromBody] GoogleLoginRequest model)
        {
            var idtoken = model.idToken;
            var setting = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new string[] { "162022528095-s5kjo980evm1u9ikbiljoolj1l4h1767.apps.googleusercontent.com" }
            };

            var result = await GoogleJsonWebSignature.ValidateAsync(idtoken, setting);
            if (result is null)
            {
                Console.WriteLine("ingresa");
                return BadRequest();
            }
            var token = GetJWTToken(result.Email);
            _tokenManager.SetToken(token);
            string tokenS = _tokenManager.GetToken();
            Console.WriteLine("token generates " + tokenS);
            return Ok(new { token = token, IsAuthSuccessful = true });
        }

        [HttpPost("tokenf")]
        public async Task<ActionResult> Facebooklogin([FromBody] FacebookLoginRequest model)
        {
            var accesstoken = model.idToken; // cambiar a propiedad de acceso a token de facebook
            var facebookappid = "932377521528711";
            var facebookappsecret = "c02d33328b51043072abd80388383550";

            var client = new HttpClient();
             var apptoken = $"{facebookappid}|{facebookappsecret}";
            //var apptoken = $"{facebookappid}";
            var url = $"https://graph.facebook.com/debug_token?input_token={accesstoken}&access_token={apptoken}";

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responsebody = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responsebody);

                if (!json.RootElement.TryGetProperty("data", out var data) || !data.TryGetProperty("is_valid", out var isvalid) || !isvalid.GetBoolean())
                {
                    Console.WriteLine("no se pudo validar el token de facebook.");
                    return BadRequest();
                }

                // si llegamos aquí, el token es válido
                var userid = data.GetProperty("user_id").GetString();
                var token = GetJWTToken(userid);
                _tokenManager.SetToken(token);
                return Ok(new { token = token, IsAuthSuccessful = true });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"error al validar el token de facebook: {ex.Message}");
                return BadRequest();
            }
        }


    }
}     

