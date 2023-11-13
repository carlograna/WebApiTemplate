using System.Text;
using System.Collections.Generic;

namespace WebApiTemplate.Validation
{
    public class NamesNotProviderException : Exception
    {
        public override string Message => "Name can't be empty";
    }
}
