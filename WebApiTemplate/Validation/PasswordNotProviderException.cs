namespace WebApiTemplate.Validation
{
    public class PasswordNotProviderException : Exception
    {
        public override string Message => "Password can't be empty";
    }
}
