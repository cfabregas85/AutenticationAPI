using AutenticationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticationAPI.Services
{
    public class TransferMoney
    {
        #region Variables

        private readonly IValidateTransfer _validateTransfer;

        #endregion

        #region Ctro

        public TransferMoney(IValidateTransfer validateTransfer)
        {
            this._validateTransfer = validateTransfer;
        }

        #endregion

        #region Methods

        public void Transfer(Account orgin, Account destination, decimal amountTransfer)
        {
            var error = _validateTransfer.TransferValidation(orgin, amountTransfer);

            if (!string.IsNullOrEmpty(error))
            {
                throw new ApplicationException(error);
            }
            orgin.Availablefunds -= amountTransfer;
            destination.Availablefunds += amountTransfer;
        }
        #endregion




    }
}
