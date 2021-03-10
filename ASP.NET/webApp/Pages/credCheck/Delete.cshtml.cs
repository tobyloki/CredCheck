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
    public class DeleteModel : PageModel
    {
        //private readonly ConsoleApp.Data.cardsContext _context;
        public string response = "";

        //public IList<cards> Card { get; set; }

        public DeleteModel(/*ConsoleApp.Data.cardsContext context*/)
        {
            //_context = context;
        }

        public class DeleteResponseData
        {
            public string data { get; set; }
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
                    var data = await httpClient.DeleteAsync(url);
                    var cardData = JsonConvert.DeserializeObject<DeleteResponseData>(data.ToString());
                    ModelState.Clear();
                    if (cardData.success)
                    {
                        response = "Your card was removed from the database";
                    } else
                    {
                        response = "Error deleting your card";
                    }
                } catch (Exception){
                    response = "There was an error adding your data";
                }
            }
        }
    }
}
