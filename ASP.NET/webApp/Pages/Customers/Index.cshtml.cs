using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConsoleApp.Data;
using ConsoleApp.Models;
using MySql.Data.MySqlClient;

namespace webApp.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ConsoleApp.Data.ContosoPetsContext _context;

        public IndexModel(ConsoleApp.Data.ContosoPetsContext context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; }

        public async Task OnGetAsync()
        {
            Customer = await _context.Customers.ToListAsync();
            using var connection = new MySqlConnection("server=localhost;port=3306;database=credcheck;user=root;password=password");

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM cards;", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var value = reader.GetValue(0);
                System.Diagnostics.Debug.WriteLine(value);
            }
        }
    }
}
