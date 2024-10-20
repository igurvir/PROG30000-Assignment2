namespace API.Models;
public class Tag
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty; // Initialize to avoid null warning
    public List<Image> Images { get; set; } = new List<Image>(); // Initialize to avoid null warning
}
