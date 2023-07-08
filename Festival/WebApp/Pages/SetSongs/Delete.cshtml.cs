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
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SetSong SetSong { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SetSongs == null)
            {
                return NotFound();
            }

            var setsong = await _context.SetSongs.FirstOrDefaultAsync(m => m.Id == id);

            if (setsong == null)
            {
                return NotFound();
            }
            else 
            {
                SetSong = setsong;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SetSongs == null)
            {
                return NotFound();
            }
            var setsong = await _context.SetSongs.FindAsync(id);

            if (setsong != null)
            {
                SetSong = setsong;
                _context.SetSongs.Remove(SetSong);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
