using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Songs
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Song> Songs { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SetId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }
        
        public async Task OnGetAsync()
        {
            if (_context.Songs == null)
            {
                return;
                Songs = await _context.Songs
                .Include(s => s.Category).ToListAsync();
            }
            
            
            var query = _context.Songs!
                .Include(s => s.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Search))
            {
                Search = Search.Trim().ToUpper();
                query = query.Where(s => 
                    s.SongName.ToUpper().Contains(Search) ||
                    s.Composer.ToUpper().Contains(Search) ||
                    s.Performer.ToUpper().Contains(Search) ||
                    s.LyricArtist.ToUpper().Contains(Search) ||
                    s.Category!.CategoryName.ToUpper().Contains(Search)
                );
            }

            Songs = await query.ToListAsync();
        }
    }
}
