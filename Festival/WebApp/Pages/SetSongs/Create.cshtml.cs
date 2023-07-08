using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Microsoft.VisualBasic.CompilerServices;

namespace WebApp.Pages.SetSongs
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? SetId { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? SongId { get; set; }
        public IActionResult OnGet()
        {
            if (SetId != null && SongId != null)
            {
                SetSong setSong = new SetSong()
                {
                    SetId = IntegerType.FromString(SetId),
                    SongId = IntegerType.FromString(SongId)
                };
                _context.SetSongs.Add(setSong);
                _context.SaveChangesAsync();
                return RedirectToPage("../Sets/Select", routeValues: new {id = SetId});
            }
            
            ViewData["SetId"] = new SelectList(_context.Sets, "Id", "SetName");
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "SongName");
            return Page();
        }

        [BindProperty] public SetSong SetSong { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.SetSongs == null || SetSong == null)
            {
                Console.WriteLine("SHOWING PAGE");
                return Page();
            }

            _context.SetSongs.Add(SetSong);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}