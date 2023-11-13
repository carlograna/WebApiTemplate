using WebApiTemplate.Database;
using WebApiTemplate.Services;
using WebApiTemplate.Request;
using Microsoft.EntityFrameworkCore;
using WebApiTemplate.Validation;

namespace XUnitTest
{
    public class AuthServiceTest
    {

        [Fact]
        public void databaseConnectionTest()
        {
            // Configura la cadena de conexión a la base de datos de prueba
            var connectionString = "server=localhost;database=PRUEBA;integrated security=true;";

            // Configura las opciones del contexto de Entity Framework
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var dbContext = new UserContext(options))
            {
                // Realiza consultas a la base de datos para obtener datos de prueba
                var resultado = dbContext.User.FirstOrDefault();

                // Realiza afirmaciones para comprobar el resultado de la prueba
                Assert.NotNull(resultado);
                // Otras afirmaciones...
            }
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsTrue()
        {
            var connectionString = "server=localhost;database=PRUEBA;integrated security=true;";

            // Configura las opciones del contexto de Entity Framework
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var dbContext = new UserContext(options))
            {
                var result = false;


                // Arrange
                var userService = new UserService(); // Instantiate your AuthService or mock it if it has dependencies
                var userLogin = new LoginRequest();
                userLogin.Username = "rregina";
                userLogin.Password = "admin123";
                //UserContext _context;

                // Act
                var usSer = userService.LoginJwt(userLogin, dbContext).Result;
                if (usSer != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                // Assert
                Assert.True(result); // Assert that the login is successful
            }
        }
        [Fact]
        public void Login_InvalidCredentials_ReturnsFalse()
        {
            var connectionString = "server=localhost;database=PRUEBA;integrated security=true;";

            // Configura las opciones del contexto de Entity Framework
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var dbContext = new UserContext(options))
            {
                var result = false;
                // Arrange
                var userService = new UserService(); // Instantiate your AuthService or mock it if it has dependencies
                var userLogin = new LoginRequest();
                userLogin.Username = "rregina";
                userLogin.Password = "admin123";

                // Act
                var usSer = userService.LoginJwt(userLogin, dbContext).Result;
                if (usSer != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                // Assert
                Assert.False(result); // Assert that the login fails

            }
        }

        [Fact]
        public void ValidateValidEmail()
        {
            var connectionString = "server=localhost;database=PRUEBA;integrated security=true;";

            // Configura las opciones del contexto de Entity Framework
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseSqlServer(connectionString)
                .Options;

            using (var dbContext = new UserContext(options))
            {
                //Arrange
                var mailValidator = new UserValidation();
                string email = "algomail.com";


                /// Act
                bool isValid = mailValidator.IsValidEmail(email);

                //Asseert
                Assert.True(isValid);
            }

        }


    }
}
