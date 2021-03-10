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
    public class UpdateModel : PageModel
    {
        //private readonly ConsoleApp.Data.cardsContext _context;
        public string response = "";

        //public IList<cards> Card { get; set; }

        public UpdateModel(/*ConsoleApp.Data.cardsContext context*/)
        {
            //_context = context;
        }

        public class PostResponseData
        {
            public string data { get; set; }
            public bool success { get; set; }
            public string message { get; set; }
        }

        public async void OnPost()
        {
            bool validData = true;
            // Validate number: only integers, Luhn, apply dashes
            string cardNumber = Request.Form["cardNumber"];
            if (!OnlyNumbers(cardNumber) || !Luhn(cardNumber))
            {
                response = "Invalid card number! ";
                validData = false;
            } else {
                cardNumber = cardNumber.Insert(12, " - ");
                cardNumber = cardNumber.Insert(8, " - ");
                cardNumber = cardNumber.Insert(4, " - ");
            }
            // Validate date: valid month/day, ensure " / " between numbers
            string expirationDate = Request.Form["expirationDate"];
            expirationDate = GetNumbers(expirationDate);
            int monthDigits = 2; //month can be 1 or 2 digits
            if (expirationDate.Length == 3)
                monthDigits = 1;
            if (!ValidDate(expirationDate, monthDigits))
            {
                response += "Invalid expiration date! ";
                validData = false;
            } else {
                expirationDate = expirationDate.Insert(monthDigits, " / ");
            }
            // validate cvv: only integers
            string cvv = Request.Form["cvv"];
            if (!OnlyNumbers(cvv))
            {
                response += "Invalid cvv! ";
                validData = false;
            }
            if (validData){  //add card data to the database
                var httpClient = HttpClientFactory.Create();
                var url = "http://localhost:8081/card";
                var shortCardNum = cardNumber.Substring(cardNumber.Length - 8);
                string cardId = Request.Form["cardId"];
                var configs = new[]
                {
                    new {  cardId = cardId, expirationDate = expirationDate, cvv = cvv, cardNumber = shortCardNum  }  //get last 8 numbers
                };
                var jsonData = new StringContent(JsonConvert.SerializeObject(configs[0]), Encoding.UTF8, "application/json");
                try{
                    var data = await httpClient.PutAsync(url, jsonData);
                    var dataBody = await data.Content.ReadAsStringAsync();
                    var cardData = JsonConvert.DeserializeObject<PostResponseData>(dataBody.ToString());
                    ModelState.Clear();
                    if (cardData.success)
                    {
                        response = " Your card's data was updated in the database!";
                    }else{
                        response = "Error: " + cardData.message;
                    }
                } catch (Exception){
                    response = "There was an error adding your data";
                }
            }
        }

        private bool OnlyNumbers(string value)
        {
            for(int i = 0; i < value.Length; i++)
                if (!(value[i] >= '0' && value[i] <= '9'))
                    return false;
            return true;
        }

        private bool Luhn(string value)
        {
            int sum = 0;
            bool isSecond = false;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                int d = int.Parse(value.Substring(i, 1));
                if (isSecond)
                    d *= 2;
                sum += d / 10;
                sum += d % 10;
                isSecond = !isSecond;
            }
            System.Diagnostics.Debug.WriteLine("Luhn Sum: " + sum);
            return (sum % 10 == 0);
        }

        private string GetNumbers(string value)
        {
            string result = "";
            for (int i = 0; i < value.Length; i++)
                if (value[i] >= '0' && value[i] <= '9')
                    result += value[i];
            return result;
        }

        private bool ValidDate(string value, int monthDigits)
        {
            if (value.Length != 4 && value.Length != 3)
                return false;
            string month = value.Substring(0, monthDigits);
            if (!(int.Parse(month) <= 12 && int.Parse(month) != 0)) // if invalid month
                return false;
            //string year = value.Substring(monthDigits, 2);
            return true;
        }

        

        /*public async Task OnGetAsync()
        {
            // GET request
            using var client = new HttpClient();
            string request_url = "http://webcode.me";
            var content = await client.GetAsync(request_url);
            //var data = {json object};
            //var content = await client.PostAsync(request_url, data);
            System.Diagnostics.Debug.WriteLine(content);

            /* 
             *  GET: get data for one card
             *  POST: get data for all cards
             *  PUT: update data  -- all but card Id
             *  DELETE: delete -- just card Id
            *

            // Collect and display card data from database
            Card = new List<cards>();
            //Card = await _context.cards.ToListAsync();
            using var connection = new MySqlConnection("server=localhost;port=3306;database=credcheck;user=root;password=password");

            await connection.OpenAsync();

            //using var command = new MySqlCommand("SELECT * FROM cards;", connection);
            string sqlText = @"SELECT * FROM cards WHERE cardId = @recordId";
            MySqlCommand command = new MySqlCommand(sqlText, connection);
            int recordId = 1;
            command.Parameters.AddWithValue("@recordId", recordId);
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                System.Diagnostics.Debug.WriteLine(reader.GetString(0));
                cards c = new cards();
                c.cardId = Int32.Parse(reader.GetString(0));
                c.expirationDate = reader.GetString(1);
                c.cardNumber = reader.GetString(2);
                c.cvv = reader.GetString(3);
                System.Diagnostics.Debug.WriteLine(c.getData());
                if(c != null)
                    Card.Add(c);
            }

        }

        private MySqlConnection GetConnection()
        {
            throw new NotImplementedException();
        }*/
    }
}
