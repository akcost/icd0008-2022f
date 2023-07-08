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

namespace WebApp.Pages.Songs
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Song Song { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public string? SetId { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("SetId: " + SetId);
          if (!ModelState.IsValid || _context.Songs == null || Song == null)
            {
                return Page();
            }

            _context.Songs.Add(Song);


            if (SetId != null)
            {
                await _context.SaveChangesAsync();
                SetSong setSong = new SetSong()
                {
                    SetId = IntegerType.FromString(SetId),
                    SongId = Song.Id
                };
                _context.SetSongs.Add(setSong);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Sets/Select", routeValues: new {id = SetId});
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
