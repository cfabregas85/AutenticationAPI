using AutenticationAPI.Models;
using AutenticationAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestIntegrationAPI.Mocks
{
    public class CardServiceMock : ICardService
    {
        public Task<Card> AddCard(Card card)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CardExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteCard(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Card>> GetAllCards()
        {
            throw new NotImplementedException();
        }

        public async Task<Card> GetCarById(int id)
        {
            if (id == 0){ return null; }

            return new Card() {
                CardId = id,
                CardOwnerName = "Alex"
            };
        }

        public Task<Card> UpdateCard(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
