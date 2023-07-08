namespace Domain;

public class Song
{
    public int Id { get; set; }
    public string SongName { get; set; } = default!;
    public string Composer { get; set; } = default!;
    public string Performer { get; set; } = default!;
    public string LyricArtist { get; set; } = default!;
    public int Length { get; set; } = default!;
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public ICollection<SetSong>? SetSongs { get; set; }
}