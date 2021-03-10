using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ConsoleApp.Data;
using ConsoleApp.Models;

namespace WebApp.Pages.ProductOrders
{
    public class IndexModel : PageModel
    {
        private readonly ConsoleApp.Data.ContosoPetsContext _context;

        public IndexModel(ConsoleApp.Data.ContosoPetsContext context)
        {
            _context = context;
        }

        public IList<ProductOrder> ProductOrder { get;set; }

        public async Task OnGetAsync()
        {
            ProductOrder = await _context.ProductOrders
                .Include(p => p.Order)
                .Include(p => p.Product).ToListAsync();
        }
    }
}
