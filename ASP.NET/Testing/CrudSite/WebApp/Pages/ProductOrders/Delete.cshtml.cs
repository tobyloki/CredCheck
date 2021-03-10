﻿using System;
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
    public class DeleteModel : PageModel
    {
        private readonly ConsoleApp.Data.ContosoPetsContext _context;

        public DeleteModel(ConsoleApp.Data.ContosoPetsContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductOrder = await _context.ProductOrders.FindAsync(id);

            if (ProductOrder != null)
            {
                _context.ProductOrders.Remove(ProductOrder);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
