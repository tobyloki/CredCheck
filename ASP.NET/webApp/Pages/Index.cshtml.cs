using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using System.IO;

namespace webApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string response = "";
        public string containerResponse = "No_response...";
        public string res1 = "res1";
        public string res2 = "res2";
        public string res3 = "res3";
        private static readonly HttpClient client = new HttpClient();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            //getRequest();
        }

        public async void getRequest(){
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/weatherforecast");
            res1 = "Request to: example.com";
            try{
                // res2 = await client.GetStringAsync("example.com");
                var uri = "example.com";
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode) {
                    res2 = await response.Content.ReadAsStringAsync();
                }
            }catch(Exception e){
                _logger.LogInformation("Exception: " + e);
                res3 = "There was an error!";
            }
            // request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            // using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            // using(Stream stream = response.GetResponseStream())
            // using(StreamReader reader = new StreamReader(stream))
            // {
            //     containerResponse = reader.ReadToEnd();
            // }
        }

        public async void getContainerResponse(){
            using (var client = new HttpClient())
            {
                // Arrange
                client.BaseAddress = new Uri("http://localhost:8080/weatherforecast");
                res1 = "Request to: http://localhost:8080/weatherforecast";
                // System.Diagnostics.Debug.WriteLine("Request to: http://localhost:8080/weatherforecast");
                // Act
                var response = await client.GetAsync(client.BaseAddress);
                res2 = "Init Response of: " + response;
                // System.Diagnostics.Debug.WriteLine("Init Response of: " + response);
                containerResponse = await response.Content.ReadAsStringAsync();
                res3 = "String Response of: " + containerResponse;
                // System.Diagnostics.Debug.WriteLine("String Response of: " + containerResponse);
            }
        }

        public void OnGet()
        {
            System.Diagnostics.Debug.WriteLine("Hiding Panel1");
            //Panel1.Visible = false;
        }

        public void OnPost()
        {
            System.Diagnostics.Debug.WriteLine("Showing Panel1");
            //Panel1.Visible = true;
            var emailAddress = Request.Form["emailaddress"];
            System.Diagnostics.Debug.WriteLine("Email: " + emailAddress);
            response = "Thank you for your info!";
        }
    }
}
