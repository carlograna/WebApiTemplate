//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using WebApiTemplate.Database;

//namespace WebApiTemplate.Model
//{
//    public class Jwt
//    {
//        public string key { get; set; }
//        public string Issuer { get; set; }
//        public string Audience { get; set; }

//        public string Subject { get; set; }


//        public static dynamic validateToken(ClaimsIdentity identify, List<User1> user)
//        {
//            try
//            {
//                if (identify.Claims.Count() == 0)
//                {
//                    return new
//                    {
//                        success = false,
//                        message = "Check if you are sending a valid token",
//                        result = ""
//                    };
//                }


//                var id = identify.Claims.FirstOrDefault(x => x.Type == "Email").Value;
//                User1 userSearch = user.FirstOrDefault(x => x.Email.Equals(Int32.Parse(id))) ?? null;

//                return new
//                {
//                    success = true,
//                    message = "Success",
//                    result = userSearch
//                };
//            }
//            catch (Exception ex)
//            {
//                return new
//                {
//                    success = false,
//                    message = "Catch: " + ex.Message,
//                    result = ""
//                };
//            }
//        }


//    }
//}