using AutenticationAPI.Models;

namespace AutenticationAPI.Services
{
    public interface IAccountService
    {
        UserToken BuildToken(UserInfo userInfo);
    }
}