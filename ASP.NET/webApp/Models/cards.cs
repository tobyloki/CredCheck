using System;
using System.Collections.Generic;
using System.Text;
using webApp.Models;

namespace appData.Models
{
    public class cards
    {
        private cardsContext context;
        public int cardId { get; set; }

        public string cvv { get; set; }

        public string expirationDate { get; set; }

        public string cardNumber { get; set; }
    }
}
