using System.ComponentModel.DataAnnotations;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DAL.AppDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IList<Set> Sets { get;set; } = default!;
    public List<Dj> Djs { get;set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }
    
    public async Task OnGetAsync()
    {

        var query = _context.Sets
            .Include(set => set.Dj)
            .AsQueryable();

        if (!string.IsNullOrEmpty(Search))
        {
            Search = Search.Trim().ToUpper();
            query = query.Where(s => 
                s.SetName.ToUpper().Contains(Search) ||
                s.Dj!.DjName.ToUpper().Contains(Search) ||
                s.SetSongs!.Any(ss => ss.Song!.SongName.ToUpper().Contains(Search)) ||
                s.SetSongs!.Any(ss => ss.Song!.Composer.ToUpper().Contains(Search)) ||
                s.SetSongs!.Any(ss => ss.Song!.Performer.ToUpper().Contains(Search)) ||
                s.SetSongs!.Any(ss => ss.Song!.LyricArtist.ToUpper().Contains(Search))
            );
        }

        Sets = await query.ToListAsync();
        
        Djs = await _context.Djs.ToListAsync();

    }
    
    
}