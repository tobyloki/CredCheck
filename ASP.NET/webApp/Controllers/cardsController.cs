using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace webApp.Controllers
{
    public class cardsController : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            //cardsContext context = HttpContext.RequestServices.GetService(typeof(webApp.Models.cardsContext)) as cardsContext;

            using var connection = new MySqlConnection("server=localhost;port=3306;database=credcheck;user=root;password=password");

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM cards;", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var value = reader.GetValue(0);
                System.Diagnostics.Debug.WriteLine(value);
            }

            return View();
        }
    }
}
