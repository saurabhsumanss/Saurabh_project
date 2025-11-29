using LibraryAPI.Models;

namespace LibraryAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
