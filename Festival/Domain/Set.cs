namespace Domain;

public class Set
{
    public int Id { get; set; }
    public string SetName { get; set; } = default!;
    public int DjId { get; set; }
    public Dj? Dj { get; set; } = default!;
    public ICollection<SetSong>? SetSongs { get; set; }

}