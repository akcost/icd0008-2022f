using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.Djs
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Dj> Djs { get;set; } = default!;

        public int TotalLength = 0;
        public int TotalSongs = 0;

        public async Task OnGetAsync()
        {
            if (_context.Djs != null)
            {
                Djs = await _context.Djs
                    .Include(dj => dj.Sets)!
                    .ThenInclude(s => s.SetSongs)!
                    .ThenInclude(ss => ss.Song)
                    .ToListAsync();
                await _context.SaveChangesAsync();
            }
        }
        
        public string GetFormattedSongTime(int length)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(length);
            return timeSpan.ToString();
        }
        
        public string GetTotalSetLength(Dj dj)
        {
            TotalLength = 0;
            TotalSongs = 0;
            if (dj.Sets == null)
            {
                return "0";
            }

            foreach (var set in dj.Sets!)
            {
                foreach (var setSong in set.SetSongs!)
                {
                    TotalSongs += 1;
                    TotalLength += setSong.Song!.Length;
                }
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(TotalLength);
            return timeSpan.ToString();
        }
        
        
    }
}
