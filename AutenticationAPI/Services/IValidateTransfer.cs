using AutenticationAPI.Models;

namespace AutenticationAPI.Services
{
    public interface IValidateTransfer
    {
        string TransferValidation(Account orgin, decimal amountTransfer);
    }
}