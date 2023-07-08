using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using Domain;

namespace WebApp.Pages.CheckersGames
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CheckersOptionId"] = new SelectList(_context.CheckersOptions, "Id", "Name");
            return Page();
        }

        [BindProperty] public CheckersGame CheckersGame { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.CheckersGames == null || CheckersGame == null)
            {
                return Page();
            }

            _context.CheckersGames.Add(CheckersGame);
            await _context.SaveChangesAsync();

            if (CheckersGame.Player2Type == EPlayerType.Ai)
            {
                return RedirectToPage("./Play", new
                {
                    id = CheckersGame.Id,
                    playerNo = 0
                });
            }
            
            if (CheckersGame.Player1Type == EPlayerType.Ai)
            {
                return RedirectToPage("./Play", new
                {
                    id = CheckersGame.Id,
                    playerNo = 1
                });
            }
            
            return RedirectToPage("./LaunchGame", new { id = CheckersGame.Id });
        }
    }
}