using AutenticationAPI.Controllers;
using AutenticationAPI.Models;
using AutenticationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTestControllerAPI
{
    [TestClass]
    public class CardControllerUnitTest
    {
        [TestMethod]
        public void ActionResultGetCardCardDoNotexistReturn404()
        {
            // Arrange

            var cardId = 1;
            var mockLogService = new Mock<ILogService>();
            var mockCardService = new Mock<ICardService>();
            mockCardService.Setup(x => x.GetCarById(cardId)).ReturnsAsync(default(Card));          
            var cardController = new CardController(mockCardService.Object, mockLogService.Object);

            // Act
            var result = cardController.GetCard(cardId).Result;

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void ActionResultGetCardReturnCard()
        {
            // Arrange
            var cardMock = new Card()
            {
                CardId = 33,
                CardOwnerName = "Alex"
            };
            
            var mockLogService = new Mock<ILogService>();
            var mockCardService = new Mock<ICardService>();            
            mockCardService.Setup(x => x.GetCarById(cardMock.CardId)).ReturnsAsync(cardMock);
            var cardController = new CardController(mockCardService.Object, mockLogService.Object);

            // Act
            var result = cardController.GetCard(cardMock.CardId).Result;

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(result.Value.CardId, cardMock.CardId);
            Assert.AreEqual(result.Value.CardOwnerName, cardMock.CardOwnerName);

        }
    }
}
