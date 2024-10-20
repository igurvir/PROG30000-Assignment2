namespace API.Models;
public class Tag
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty; 
    public List<Image> Images { get; set; } = new List<Image>(); 
}
