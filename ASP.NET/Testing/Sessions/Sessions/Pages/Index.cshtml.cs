using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sessions.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IHttpClientFactory _clientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public string message { get; set; }

        public void OnGet()
        {
            var user = Request.Query["user"];
            if (!string.IsNullOrEmpty(user))
            {
                HttpContext.Session.SetString("currentUser", user);
            }

            var currentUser = HttpContext.Session.GetString("currentUser");
            if(currentUser == "alex")
            {
                message = "Authenticated";
            } else
            {
                message = $"{currentUser} isn't allowed on this page";
            }
        }
    }
}
