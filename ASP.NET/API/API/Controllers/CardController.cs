using API.Models;
using API.Models.Dtos.Card;
using API.Services.CardService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController: ControllerBase
    {

        private readonly ICardService _cardService;

        public CardController(ICardService cardService )
        {
            _cardService = cardService;
        }

        [Route("GetALL")]
        public async Task<IActionResult> Get()
        {
            //
            return Ok(await _cardService.GetAllCards());
        }
        // post: post card
        // put: edit card
        // DElete: delete card
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(string id)
        {
            return Ok(await _cardService.GetCardById(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> AddCard(AddCardDto newCard) // postman provides body with parameters
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            if (newCard.cardNumber == "0" || newCard.cvv == "0" || newCard.expirationDate == "0")
            {
                response.Success = false;
                response.Message = "Body did not match required body";
                return NotFound(response);
            }
            response = await _cardService.AddCard(newCard);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditCard(EditCardDto EditedCard) // postman provides body with parameters
        {
            ServiceResponse<GetCardDto> response = new ServiceResponse<GetCardDto>();
            if (EditedCard.cardNumber=="0" || EditedCard.cardId=="0" || EditedCard.cvv == "0" || EditedCard.expirationDate == "0")
            {
                response.Success = false;
                response.Message = "Body did not match required body";
                return NotFound(response);
            }
            else {
                response = await _cardService.EditCard(EditedCard);
                if (!response.Success)
                    return NotFound(response);
                return Ok(response);
            }
             
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            ServiceResponse<GetCardDto> response = await _cardService.DeleteCard(id);
            response.Message = response.Message;
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }
    }
}
