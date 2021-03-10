using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConsoleApp.Data;
using ConsoleApp.Models;

namespace WebApp.Pages.ProductOrders
{
    public class EditModel : PageModel
    {
        private readonly ConsoleApp.Data.ContosoPetsContext _context;

        public EditModel(ConsoleApp.Data.ContosoPetsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductOrder ProductOrder { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductOrder = await _context.ProductOrders
                .Include(p => p.Order)
                .Include(p => p.Product).FirstOrDefaultAsync(m => m.Id == id);

            if (ProductOrder == null)
            {
                return NotFound();
            }
           ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
           ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ProductOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductOrderExists(ProductOrder.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductOrderExists(int id)
        {
            return _context.ProductOrders.Any(e => e.Id == id);
        }
    }
}
