using AutenticationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticationAPI.Services
{
    public class ValidateTransfer : IValidateTransfer
    {
        public string TransferValidation(Account orgin, decimal amountTransfer)
        {
            if (amountTransfer > orgin.Availablefunds)
            {
                throw new ApplicationException("There are not available funds");
            }
            return String.Empty;
        }
    }
}