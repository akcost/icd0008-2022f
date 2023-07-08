using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Sets
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

      public Set Set { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Sets == null)
            {
                return NotFound();
            }

            var set = await _context.Sets.FirstOrDefaultAsync(m => m.Id == id);
            if (set == null)
            {
                return NotFound();
            }
            else 
            {
                Set = set;
            }
            return Page();
        }
    }
}
