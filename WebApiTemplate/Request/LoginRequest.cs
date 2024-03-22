using Newtonsoft.Json;

namespace WebApiTemplate.Request
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class GoogleLoginRequest
    {
        public string idToken { get; set; }

    }

    ///   agrergue esto   /// 
    public class FacebookLoginRequest
    {
         public string idToken { get; set; }
    }



    public class TokenManager
    {
        private string _token;

        public void SetToken(string token)
        {
            _token = token;
        }

        public string GetToken()
        {
            return _token;
        }
    }


}
