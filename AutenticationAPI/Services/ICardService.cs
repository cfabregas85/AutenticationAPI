using System.Collections.Generic;
using System.Threading.Tasks;
using AutenticationAPI.Models;

namespace AutenticationAPI.Services
{
    public interface ICardService
    {
        Task<Card> AddCard(Card card);
        Task<bool> CardExists(int id);
        Task<string> DeleteCard(int id);
        Task<List<Card>> GetAllCards();
        Task<Card> GetCarById(int id);        
        Task<Card> UpdateCard(Card card);
    }
}