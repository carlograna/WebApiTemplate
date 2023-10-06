using FluentValidation;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Text.RegularExpressions;
using WebApiTemplate.Database;

namespace WebApiTemplate.Validation
{
    public class UserValidation : AbstractValidator<User>
    {

        public bool NotEmptyNames(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new NamesNotProviderException();
            return true;
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new NamesNotProviderException();

            if (password.Length > 8)
            {
                return true;
            }
            throw new PasswordLenghtException();
            return false;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new EmailNotProviderException();

            Regex regex = new Regex(@"^[\w0-9._%+-]+@[\w0-9.-]+\.[\w]{2,6}$");
            return regex.IsMatch(email);
        }
    }
}
