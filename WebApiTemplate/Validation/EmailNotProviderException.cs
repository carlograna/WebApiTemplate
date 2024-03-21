namespace WebApiTemplate.Validation
{
    public class EmailNotProviderException : Exception
    {
        public override string Message => "Email can't be empty";
    }
}
