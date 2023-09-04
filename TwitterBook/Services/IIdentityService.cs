using System.Threading.Tasks;
using TwitterBook.Domain;

namespace TwitterBook.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}