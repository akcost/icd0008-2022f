using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.SetSongs
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<SetSong> SetSong { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SetSongs != null)
            {
                SetSong = await _context.SetSongs
                .Include(s => s.Set)
                .Include(s => s.Song).ToListAsync();
            }
        }
    }
}
