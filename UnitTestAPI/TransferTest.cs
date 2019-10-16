using AutenticationAPI.Models;
using AutenticationAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTestAPI
{
    [TestClass]
    public class TransferTest
    {
        //Unit Test Using Moq

        [TestMethod]
        public void Transfernotavailablefunds()
        {
            Exception expectedException = null;
            Account orgin = new Account() { Availablefunds = 0 };
            Account destination = new Account() { Availablefunds = 0 };
            decimal amountTransfer = 5m;

            var mock = new Mock<IValidateTransfer>();
            string error = "There are not available funds";
            mock.Setup(x => x.TransferValidation(orgin, amountTransfer)).Returns(error);
            var service = new TransferMoney(mock.Object);

            try
            {
                service.Transfer(orgin, destination, amountTransfer);
                Assert.Fail("Error");
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            Assert.IsTrue(expectedException is ApplicationException);     
            Assert.AreEqual(error, expectedException.Message);
        }

        [TestMethod]
        public void TransferSuccess()
        {
            Account orgin = new Account() { Availablefunds = 10 };
            Account destination = new Account() { Availablefunds = 5 };
            decimal amountTransfer = 7m;
            var mock = new Mock<IValidateTransfer>();
            mock.Setup(x => x.TransferValidation(orgin, amountTransfer)).Returns(string.Empty);
            var service = new TransferMoney(mock.Object);

            service.Transfer(orgin, destination, amountTransfer);          

            Assert.AreEqual(3, orgin.Availablefunds);
            Assert.AreEqual(12, destination.Availablefunds);
        }
    }
}
