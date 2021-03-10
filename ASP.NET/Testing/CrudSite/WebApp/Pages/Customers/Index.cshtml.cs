using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConsoleApp.Data;
using ConsoleApp.Models;

namespace WebApp.Pages.Customers
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
        }
    }
}
