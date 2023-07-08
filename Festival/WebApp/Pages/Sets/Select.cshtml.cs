using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Sets;

public class Select : PageModel
{
    private readonly AppDbContext _context;

    public int? SetId { get; set; }

    public Set? ThisSet { get; set; }

    public int TotalLength = 0;

    public Select(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id != null)
        {
            SetId = id.Value;
            ThisSet = await _context.Sets
                .Include(s => s.SetSongs!)
                .ThenInclude(ss => ss.Song)
                .Include(s => s.Dj)
                .FirstOrDefaultAsync(s => s.Id == SetId.Value);
        }


        return Page();
    }

    public string GetFormattedSongTime(int length)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(length);
        return timeSpan.ToString("mm\\:ss");;
    }
    
    public string GetTotalSetLength(ICollection<SetSong>? aSetSongs)
    {
        TotalLength = 0;
        if (aSetSongs == null)
        {
            return "0";
        }

        foreach (var setSong in aSetSongs)
        {
            if (setSong.Song != null)
            {
                TotalLength += setSong.Song.Length;
            }
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(TotalLength);
        return timeSpan.ToString();
    }
}
