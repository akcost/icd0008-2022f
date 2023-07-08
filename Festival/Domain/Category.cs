namespace Domain;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = default!;
    
    public ICollection<Song>? Songs { get; set; }

}