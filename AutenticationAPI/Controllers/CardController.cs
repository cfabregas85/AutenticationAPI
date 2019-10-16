using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutenticationAPI.Models;
using AutenticationAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CardController : ControllerBase
    {

        #region Variables

        private readonly ICardService _cardService;
        private readonly ILogService _logService;

        #endregion

        #region Ctro

        public CardController(ICardService cardService, ILogService logService)
        {
            _cardService = cardService;
            _logService = logService;
        }

        #endregion

        #region End Points
               
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetAllCards()
        {
            try
            {
                return await _cardService.GetAllCards();                
            }
            catch (Exception ex)
            {
                await _logService.LogMessage(ex.Message);
                return StatusCode(500);
            }           
        }
        
        [HttpGet("{id}")]        
        public async Task<ActionResult<Card>> GetCard(int id)
        {
           try
           {
                var card =  await _cardService.GetCarById(id);
                if (card == null){ return NotFound(); }
                return card;
           }
            catch (Exception ex)
            {
                await _logService.LogMessage(ex.Message);
                return StatusCode(500);
            }          
        }

        [HttpPost]
        public async Task<ActionResult<Card>> PostAddCard(Card card)
        {
            try
            {
                if (card == null){return BadRequest();}
                var result = await _cardService.AddCard(card);

                return CreatedAtAction("GetCard", new { id = card.CardId }, card);                               
            }
            catch (Exception ex)
            {
                await _logService.LogMessage(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCard(int id, Card card)
        {
            try
            {
                if (id != card.CardId || !await _cardService.CardExists(id)) { return BadRequest("holla"); }

                await _cardService.UpdateCard(card);
                return Ok();

            }
            catch (Exception ex)
            {
                await _logService.LogMessage(ex.Message);
                return StatusCode(500);
            }            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCard( int id)
        {
            try
            {
                var card = _cardService.GetCarById(id);
                if (card.Result == null) { return NotFound(); }

                await _cardService.DeleteCard(id);
                return Ok(card.Result.CardNumber);
            }
            catch (Exception ex)
            {
                await _logService.LogMessage(ex.Message);
                return StatusCode(500);
            }            
        }

        #endregion

    }
}