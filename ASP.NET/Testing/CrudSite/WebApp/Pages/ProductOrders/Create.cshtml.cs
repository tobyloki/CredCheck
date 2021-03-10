using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConsoleApp.Data;
using ConsoleApp.Models;

namespace WebApp.Pages.ProductOrders
{
    public class CreateModel : PageModel
    {
        private readonly ConsoleApp.Data.ContosoPetsContext _context;

        public CreateModel(ConsoleApp.Data.ContosoPetsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public ProductOrder ProductOrder { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ProductOrders.Add(ProductOrder);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
