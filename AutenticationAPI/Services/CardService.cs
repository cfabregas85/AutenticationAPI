using AutenticationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutenticationAPI.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AutenticationAPI.Services
{
    public class CardService : ICardService
    {
        #region Variables

        private readonly ApplicationDbContext _context;

        #endregion

        #region Ctro

        public CardService(ApplicationDbContext context)
        {
            _context = context;
        }   

        #endregion

        #region Card Services  Add/Update/Delete

        public async Task<List<Card>> GetAllCards()
        {           
          return await _context.Card.ToListAsync();           
        }

        public async Task<Card> AddCard(Card card)
        {
             _context.Card.Add(card);
             await _context.SaveChangesAsync();
             return card;                                              
        }

        public async Task<Card> UpdateCard(Card card)
        {
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<string> DeleteCard(int id)
        {           
            Card card = _context.Card.Find(id);
            _context.Card.Remove(card);
            await _context.SaveChangesAsync();
            return card.CardNumber;                    
        }

        #endregion

        #region Others Methods

        public async Task<Card> GetCarById(int id)
        {           
          return await _context.Card.FindAsync(id);           
        }

        public async Task<bool> CardExists(int id)
        {
            return await _context.Card.AnyAsync(e => e.CardId == id);
        }        

        #endregion

    }
}
