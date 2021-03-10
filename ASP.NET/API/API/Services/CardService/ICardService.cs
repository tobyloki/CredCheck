using API.Models;
using API.Models.Dtos.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.CardService
{
    public interface ICardService
    {
        Task<ServiceResponse<List<GetCardDto>>> GetAllCards();
        Task<ServiceResponse<GetCardDto>> GetCardById(string id);
        Task<ServiceResponse<string>> AddCard(AddCardDto newCard);
        Task<ServiceResponse<GetCardDto>> EditCard(EditCardDto EditedCard);
        Task<ServiceResponse<GetCardDto>> DeleteCard(string id);
    }
}
