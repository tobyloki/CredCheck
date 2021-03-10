﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
	public class card
	{
		public string cardNumber { get; set; }
		public string expirationDate { get; set; }
		public string cvv { get; set; }
		public string cardId { get; set; }

		public card(string cardNumber_, string expirationDate_, string cvv_, string cardId_)
		{
			this.cardNumber = cardNumber_;
			this.expirationDate = expirationDate_;
			this.cvv = cvv_;
			this.cardId = cardId_;
		}

        public card()
        {
			//var rng = new Random();
			cardNumber = 0.ToString(); // rng.Next(100000000, 999999999).ToString();
			expirationDate = 0.ToString();
			cvv = 0.ToString();  // rng.Next(100, 999).ToString();
			cardId = 0.ToString();
		}
    }
}
