using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.SetSongs
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
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

            var setsong =  await _context.SetSongs.FirstOrDefaultAsync(m => m.Id == id);
            if (setsong == null)
            {
                return NotFound();
            }
            SetSong = setsong;
           ViewData["SetId"] = new SelectList(_context.Sets, "Id", "SetName");
           ViewData["SongId"] = new SelectList(_context.Songs, "Id", "SongName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SetSong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SetSongExists(SetSong.Id))
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

        private bool SetSongExists(int id)
        {
          return (_context.SetSongs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
