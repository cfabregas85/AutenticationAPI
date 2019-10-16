using System.Threading.Tasks;

namespace AutenticationAPI.Services
{
    public interface ILogService
    {
        Task LogMessage(string _message);
    }
}