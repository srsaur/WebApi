using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models;

namespace AngularASPNETCore2WebApiAuth.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(AppUser user);
    }
}