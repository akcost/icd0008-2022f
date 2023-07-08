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
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Set> Set { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Sets != null)
            {
                Set = await _context.Sets
                    .Include(s => s.Dj).ToListAsync();
            }
        }
    }
}
