using API.Models.Dtos.Card;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<card, GetCardDto>();
            CreateMap<AddCardDto, card>();
        }
    }
}
