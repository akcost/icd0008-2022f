namespace Domain;

public class Dj
{
    public int Id { get; set; }
    public string DjName { get; set; } = default!;
    public int PricePerSecond { get; set; } = default!;
    public ICollection<Set>? Sets { get; set; }

}