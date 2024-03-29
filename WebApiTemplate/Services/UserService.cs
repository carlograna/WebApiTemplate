﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiTemplate.Database;
using WebApiTemplate.Request;

namespace WebApiTemplate.Services
{
    public class UserService
    {

        private readonly HttpClient _httpClient;


        public async Task<User1> LoginJwt(LoginRequest userLogin, UserContext _context)
        {
            var user = await Authenticate(userLogin, _context);
            if (user != null)
            {
                return user;

            }
            return null!;

        }
        private async Task<User1> Authenticate(LoginRequest userLogin, UserContext _context)
        {
            await Task.Delay(2000);
            var currentUser = _context.User1?.ToList().FirstOrDefault(user => user.Username.ToLower() == userLogin.Username.ToLower()
                   && user.Password == userLogin.Password); ;
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
        public JwtSecurityToken Generate(User1 user, IConfiguration _config)
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

            return new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials); ;
        }


    }
}