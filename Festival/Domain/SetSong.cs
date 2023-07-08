namespace Domain;

public class SetSong
{
    public int Id { get; set; }
    
    public int SetId { get; set; }
    public Set? Set { get; set; }

    public int SongId { get; set; }
    public Song? Song { get; set; }
}