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
    public class IndexModel : PageModel
    {
        //private readonly ConsoleApp.Data.cardsContext _context;
        public string response = "";

        //public IList<cards> Card { get; set; }

        public IndexModel(/*ConsoleApp.Data.cardsContext context*/)
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
                response = "Your card was added to the database!";
                var httpClient = HttpClientFactory.Create();
                var url = "http://localhost:8081/card";
                var shortCardNum = cardNumber.Substring(cardNumber.Length - 11);  //get last 8 numbers (plus a " - ")
                var configs = new[]
                {
                    new {  expirationDate = expirationDate, cvv = cvv, cardNumber = shortCardNum  }  
                };
                var jsonData = new StringContent(JsonConvert.SerializeObject(configs[0]), Encoding.UTF8, "application/json");
                try{
                    var data = await httpClient.PostAsync(url, jsonData);
                    var dataBody = await data.Content.ReadAsStringAsync();
                    var cardData = JsonConvert.DeserializeObject<PostResponseData>(dataBody.ToString());
                    ModelState.Clear();
                    if (cardData.success)
                    {
                        response += " Your key is " + cardData.data;
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
            return true;
        }
    }
}
