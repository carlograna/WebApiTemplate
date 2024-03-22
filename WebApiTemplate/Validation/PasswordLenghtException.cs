namespace WebApiTemplate.Validation
{
    public class PasswordLenghtException : Exception
    {
        public override string Message => "Password cannot be less than 8 characters";
    }
}
