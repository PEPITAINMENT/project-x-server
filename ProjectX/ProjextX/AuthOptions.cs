using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Server
{
    public class AuthOptions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
