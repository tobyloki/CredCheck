using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using ConsoleApp.Data;
//using ConsoleApp.Models;
using MySql.Data.MySqlClient;
//using appData.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace webApp.Pages.credCheck
{
    public class ReadModel : PageModel
    {
        //private readonly ConsoleApp.Data.cardsContext _context;
        public string response = "";

        //public IList<cards> Card { get; set; }

        public ReadModel(/*ConsoleApp.Data.cardsContext context*/)
        {
            //_context = context;
        }

        public class Card
        {
            public string cardNumber { get; set; }
            public string expirationDate { get; set; }
            public string cvv { get; set; }
        }

        public class GetResponseData
        {
            public Card data { get; set; }
            public bool success { get; set; }
            public string message { get; set; }
        }

        public async void OnPost()
        {
            // get inputted card id
            string cardId = Request.Form["cardId"];
            if (cardId != null){  //return card results
                var httpClient = HttpClientFactory.Create();
                var url = "http://localhost:8081/card" + cardId;
                try{
                    var data = await httpClient.GetStringAsync(url);
                    var cardData = JsonConvert.DeserializeObject<GetResponseData>(data.ToString());
                    ModelState.Clear();
                    if (cardData.success)
                    {
                        response = "Card Number: *********" + cardData.data.cardNumber;
                        response += "\nExpiration Date: " + cardData.data.expirationDate;
                        response += "\ncvv: " + cardData.data.cvv;
                    } else
                    {
                        response = "Error: " + cardData.message;
                    }
                } catch (Exception){
                    response = "There was an error adding your data";
                }
            }
        }
    }
}
