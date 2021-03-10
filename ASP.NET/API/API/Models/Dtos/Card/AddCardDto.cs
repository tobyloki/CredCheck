using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Dtos.Card
{
    public class AddCardDto
    {
        public string cardNumber { get; set; }
        public string expirationDate { get; set; }
        public string cvv { get; set; }
        public AddCardDto()
        {
            cardNumber = 0.ToString(); // rng.Next(100000000, 999999999).ToString();
            expirationDate = 0.ToString();
            cvv = 0.ToString();  // rng.Next(100, 999).ToString();
                // docker run -it --rm -p 8080:80 app:v1
        }
    }
    
}
